﻿using System;
using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Data.Enum;
using Source.Scripts.System.Util;
using Source.Scripts.View;
using Source.Scripts.View.Player;
using UnityEngine;

namespace Source.Scripts.System
{
    public class Fabric
    {
        private EcsWorld world;
        private readonly SaveData save;
        private readonly GameData gameData;
        private readonly GameConfig config;
        private readonly Pools pool;

        public Fabric(EcsWorld world, SaveData save, GameData gameData, GameConfig config,Pools pool)
        {
            this.world = world;
            this.save = save;
            this.gameData = gameData;
            this.config = config;
            this.pool = pool;
        }
        
        public int InitView(BaseView baseView)
        {
            var position = baseView.transform.position;
            var entity = world.NewEntity();
            baseView.Entity = entity;
            pool.View.Add(entity).Value = baseView;
            pool.Dir.Add(entity).Value = baseView.transform.forward;

            
            var rigidbody = baseView.GetComponent<Rigidbody>();
            if (rigidbody != null)
                pool.Rb.Add(entity).Value = rigidbody;
            

            var hpView = baseView.GetComponent<HpView>();
            if (hpView != null)
            {
                ref var hp = ref pool.Hp.Add(entity);
                hp.MaxHp = hp.CurHp = hpView.Value;
            }
            
            var animationView = baseView.GetComponent<AnimationView>();
            if (animationView != null)
            {
                pool.Anim.Add(entity).Value = animationView;
            }

           

            var upsView = baseView.GetComponent<UpsView>();
            if (upsView != null)
            {
                var ups = new Dictionary<UpType, int>();
                foreach (var kv in upsView.Ups)
                    ups.Add(kv.Key, kv.Value);

                if (baseView.Entity == gameData.PlayerEntity && save.Ups != null)
                    ups = save.Ups;
                else
                    save.Ups = ups;

                pool.Ups.Add(entity).Value = ups;

                if (baseView.Entity == gameData.PlayerEntity)
                {
                    ref var hp = ref pool.Hp.Get(entity);
                    hp.CurHp = hp.MaxHp = config.PlayerUps[UpType.HP][ups[UpType.HP]];
                    
                }
            }

          

            var inventoryView = baseView.GetComponent<InventoryView>();
            if (inventoryView != null)
            {
                var inventory = new Dictionary<ResType, int>();
                var values = Enum.GetValues(typeof(ResType));
                
                foreach (var item in values)
                    inventory.Add((ResType)item,0);

                if (baseView.Entity == gameData.PlayerEntity)
                {
                    if (save.PlayerInventory != null)
                        inventory = save.PlayerInventory;
                    else
                        save.PlayerInventory = inventory;
                }
                
                pool.Inventory.Add(entity).Value = inventory;
            }

            //ui
           /* var hpBarView = baseView.GetComponent<HpBarView>();
            if (hpBarView != null)
            {
                pool.HpView.Add(entity).Value = hpBarView.HpBarUIView;
            }*/
           
            return entity;
        }
    }
}