from django.urls import path
from . import views
urlpatterns = [
    path('attack_boss/<int:player_id>/<int:boss_id>/', views.attack_boss, name="attack_boss"),
    path('check_user/<int:player_id>/', views.check_user, name="check_user"),
    path('try_open_boss_table/<int:player_id>/', views.try_open_boss_table, name="try_open_boss_table"),
    path('show_boss_fight/<int:player_id>/', views.show_boss_fight, name="show_boss_fight"),
    path('get_boss/<int:player_id>/', views.get_boss, name="get_boss"),
    path('try_to_use_flask/<int:player_id>/', views.try_to_use_flask, name="try_to_use_flask"),
    path('use_flask/<int:player_id>/', views.use_flask, name="use_flask"),
    path('get_damage_board/<int:player_id>/', views.get_damage_board, name="get_damage_board"),
    path('to_damage/<int:player_id>/<int:damage_count>/', views.to_damage, name="to_damage"),
    path('get_bosses_table/', views.get_bosses_table, name="get_bosses_table"),
    path('take_id/', views.take_id, name="take_id"),
    path('get_friend/<int:player_id>/<int:friend_id>/', views.get_friend, name="get_friend"),
]
