from django.shortcuts import render
from .models import Player, Boss, AbstractBoss, BossPlayer
from django.http import JsonResponse
from .serializers import PlayerSerializer, AbstractBossSerializer, BossSerializer, RewardSerializer, BossPlayerSerializer

def check_user(request, player_id):
    try:
        player = PlayerSerializer(Player.objects.get(id = player_id)).data
        return JsonResponse(player, safe = False)
    except:
        return JsonResponse('false', safe = False)

def attack_boss(request, player_id, boss_id):
    try:
        player = Player.objects.get(id = player_id)
    except:
        return JsonResponse('Player does not exist', safe = False)
    
    if (player.active_boss is None):
        abstract_boss = AbstractBoss.objects.get(id = boss_id)
        boss = Boss.objects.create(name = abstract_boss.name, max_health = abstract_boss.max_health,
            health = abstract_boss.max_health, isReached = False, reward = abstract_boss.reward
        )
        boss.save()
        player.active_boss = boss
        player.save()
        return JsonResponse('StartFight', safe = False)
    else:
        return JsonResponse('PLayer already fights with boss', safe = False)

def try_open_boss_table(request, player_id):
    try:
        player = Player.objects.get(id = player_id)
    except:
        return JsonResponse('Player does not exist', safe = False)
    
    if player.active_boss is not None:
        if player.active_boss.health <= 0:
            player.active_boss.isReached = True
            player.active_boss.save()
        if player.active_boss.isReached:
            return JsonResponse('Win', safe = False)
        else:
            return JsonResponse('Fighting', safe = False)
    else:
        return JsonResponse('OpenTable', safe = False)

def show_boss_fight(request, player_id):
    player_boss = Player.objects.get(id = player_id).active_boss
    boss = Boss.objects.values().get(id = player_boss.id)
    player_flask_count = Player.objects.get(id = player_id).flask_count
    resp = [boss, {"flask_count" : player_flask_count}]
    return JsonResponse(resp, safe = False)


def to_damage(request, player_id, damage_count):
    try:
        player = Player.objects.get(id = player_id)
    except:
        return JsonResponse('Player does not exist', safe = False)

    if player.active_boss is not None:
        player.active_boss.take_damage(damage_count)
        try:
            bp = BossPlayer.objects.get(player = player)
        except:
            bp = BossPlayer.objects.create(player = player, damage_count = 0)
            player.active_boss.damages.add(bp)
            player.active_boss.save()
            bp.save()
        if bp in player.active_boss.damages.all():
            bp.damage_count += damage_count
            bp.save()
        player.active_boss.save()
        if player.active_boss.isReached is True:
            return JsonResponse('Win', safe = False)
        
        if player.player_friends.count() > 0:
            for friend in player.player_friends.all():
                if friend.active_boss is not None:
                    friend.active_boss.take_damage(damage_count)
                    friend.active_boss.save()
        return JsonResponse('Damage', safe = False)
    else:
        return JsonResponse('Boss not finded', safe = False)

def get_boss(request, player_id):
    player_boss = Player.objects.get(id = player_id).active_boss
    boss = Boss.objects.values().get(id = player_boss.id)
    player = Player.objects.get(id = player_id)
    player.active_boss = None
    bossplayer = BossPlayer.objects.get(player = player)
    if bossplayer is not None:
        bossplayer.delete()
    player.save()
    return JsonResponse(boss, safe = False)

def get_bosses_table(request):
    bosses = AbstractBoss.objects.all().values()
    return JsonResponse(list(bosses), safe = False)

def try_to_use_flask(request, player_id):
    player = Player.objects.get(id = player_id)
    if player.flask_count - 1 >= 0:
        return JsonResponse('true', safe = False)
    else:
        return JsonResponse('false', safe = False)

def use_flask(request, player_id):
    player = Player.objects.get(id = player_id)
    player.flask_count -= 1
    player.save()
    return JsonResponse('UseFlask', safe = False)

def take_id(request):
    taken_id = Player.objects.latest('id') + 1
    return JsonResponse(taken_id, safe = False)

def get_friend(request, player_id, friend_id):
    player = Player.objects.get(id = player_id)
    friend = Player.objects.get(id = friend_id)
    try:
        if friend in player.player_friends.all():
            friend = PlayerSerializer(friend).data
            return JsonResponse(friend, safe = False)
    except:
        return JsonResponse('false', safe = False)

def get_damage_board(request, player_id):
    player = Player.objects.get(id = player_id)
    res = []
    damages = player.active_boss.damages.order_by('damage_count')
    for bossplayer in damages:  
        res.append(BossPlayerSerializer(bossplayer).data)
    return JsonResponse(res, safe = False)