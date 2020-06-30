from django.db import models

class AbstractBoss(models.Model):
    objects = models.Manager()
    name = models.CharField('Имя босса', max_length=50, null=True)
    max_health = models.IntegerField('Максимальное здоровье', null=True)
    reward = models.ForeignKey('Reward', on_delete=models.SET_NULL, verbose_name='награда за победу', null = True)


    class Meta:
        verbose_name = 'Абстрактный Босс'
        verbose_name_plural = 'Абстрактные Боссы'
    
    def __str__(self):
        return self.name

class Boss(models.Model):
    objects = models.Manager()
    max_health = models.IntegerField('Максимальное здоровье', null=True)
    health = models.IntegerField('Текущее здоровье', null=True)
    name = models.CharField('Имя босса', max_length=50, null=True)
    isReached = models.BooleanField('Побежден ли босс?', default=False)
    reward = models.ForeignKey('Reward', on_delete=models.SET_NULL, verbose_name='награда за победу', null = True)

    class Meta:
        verbose_name = 'Босс'
        verbose_name_plural = 'Боссы'
    
    def __str__(self):
        return self.name

    def take_damage(self, damage):
        self.health -= damage
        if self.health <= 0:
            self.isReached = True


class Player(models.Model):
    objects = models.Manager()

    level = models.IntegerField('Уровень игрока', default=1)
    name = models.CharField('Имя игрока', max_length=50, null=True)
    coins = models.IntegerField('Кол-во монет', default=100)

    active_boss = models.ForeignKey(Boss, on_delete=models.SET_NULL, verbose_name='бой с боссом', null=True, blank = True)

    fist_damage = models.IntegerField('Урон с руки', default=50)
    flask_damage = models.IntegerField('Урон с колбы', default=200)
    flask_count = models.IntegerField('Кол-во колб', default=10)

    player_friends = models.ManyToManyField('Player', verbose_name='Друзья', blank = True)

    class Meta:
        verbose_name = 'Игрок'
        verbose_name_plural = 'Игроки'
    
    def __str__(self):
        return self.name

class Reward(models.Model):
    coins_count = models.IntegerField('Кол-во монет', null=True)
    flask_count = models.IntegerField('Кол-во колб', null=True)

    class Meta:
        verbose_name = 'Награда'
        verbose_name_plural = 'Награды'
    
    def __str__(self):
        return 'Награда'