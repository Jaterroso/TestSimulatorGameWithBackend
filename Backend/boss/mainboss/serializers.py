from rest_framework.serializers import ModelSerializer
from .models import Player, AbstractBoss, Reward, Boss, BossPlayer

class RewardSerializer(ModelSerializer):
    class Meta:
        model = Reward
        fields = '__all__'

class AbstractBossSerializer(ModelSerializer):
    class Meta:
        model = AbstractBoss
        fields = ('name', 'max_health')

class PlayerSerializer(ModelSerializer):
    class Meta:
        model = Player
        fields = '__all__'

class BossSerializer(ModelSerializer):
    class Meta:
        model = Boss
        fields = '__all__'

class BossPlayerSerializer(ModelSerializer):
    class Meta:
        model = BossPlayer
        fields = '__all__'