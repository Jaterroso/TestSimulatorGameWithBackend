from django.contrib import admin
from .models import Boss, Player, AbstractBoss, Reward, BossPlayer

admin.site.register(Boss)
admin.site.register(BossPlayer)
admin.site.register(AbstractBoss)
admin.site.register(Player)
admin.site.register(Reward)
