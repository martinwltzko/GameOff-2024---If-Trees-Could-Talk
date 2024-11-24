# Generated by Django 5.1.2 on 2024-11-13 18:31

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('service', '0002_message_x_message_y_message_z'),
    ]

    operations = [
        migrations.AddField(
            model_name='message',
            name='normal',
            field=models.JSONField(default={'x': 0.0, 'y': 0.0, 'z': 0.0}),
        ),
        migrations.AddField(
            model_name='message',
            name='position',
            field=models.JSONField(default={'x': 0.0, 'y': 0.0, 'z': 0.0}),
        ),
    ]