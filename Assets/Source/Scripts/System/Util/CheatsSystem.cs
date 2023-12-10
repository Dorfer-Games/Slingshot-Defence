﻿using System;
using AYellowpaper.SerializedCollections;
using Kuhpik;
using Source.Scripts.Component;
using Source.Scripts.Data.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Source.Scripts.System.Util
{
    public class CheatsSystem : GameSystem
    {
        [SerializeField] private SerializedDictionary<ElementType,Button> buttons;
        [SerializeField] private SerializedDictionary<TomeType,Button> tomeButtons;
        //[SerializeField] private Button rndButton;

        public override void OnInit()
        {
            base.OnInit();
            
            foreach (var kv in tomeButtons)
            {
                kv.Value.onClick.AddListener(() =>
                {
                    if (pool.Tomes.Get(game.PlayerEntity).Value[kv.Key] < 4)
                    {
                        pool.Tomes.Get(game.PlayerEntity).Value[kv.Key] += 1;
                        kv.Value.GetComponentInChildren<TextMeshProUGUI>().text +=
                            "\n"+pool.Tomes.Get(game.PlayerEntity).Value[kv.Key].ToString();
                    }
                });
            }
            
            foreach (var kv in buttons)
            {
                kv.Value.onClick.AddListener(() =>
                {
                    var ammo = pool.Ammo.Get(game.PlayerEntity);
                    var firstAmmo = ammo.Value[^ammo.Count];
                    pool.Element.Get(firstAmmo).Value = kv.Key;
                    ref var level =ref pool.Level.Get(firstAmmo).Value;
                    level++;
                    int modelID = (int) kv.Key;
                    if (level>4)
                    {
                        pool.Ult.GetOrCreateRef(firstAmmo).Value = (UltType) kv.Key;
                        modelID = config.ElementsCount-1+ (int)kv.Key;
                    }
                    
                    pool.ModelChangerComponent.Get(firstAmmo).Value.SetModel(modelID);
                  
                    //pool.Elements.Get(game.PlayerEntity).Value[kv.Key] = 1;
                });
            }
           /* rndButton.onClick.AddListener(() =>
            {
                var ammo = pool.Ammo.Get(game.PlayerEntity);
                var firstAmmo = ammo.Value[^ammo.Count];
                var values = Enum.GetValues(typeof(ElementType));
                var value = (ElementType)values.GetValue(Random.Range(0, values.Length));
                pool.Element.Get(firstAmmo).Value = value;
                pool.Elements.Get(game.PlayerEntity).Value[value] = 1;
                pool.ModelChangerComponent.Get(firstAmmo).Value.SetModel((int)value);
            });*/
            
        }
#if UNITY_EDITOR
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                foreach (var ent in world.Filter<Enemy>().End())
                {
                    pool.CantMove.GetOrCreateRef(ent);
                    pool.Speed.Get(ent).Value = 0;
                }
            }
        } 
#endif
        
    }
}