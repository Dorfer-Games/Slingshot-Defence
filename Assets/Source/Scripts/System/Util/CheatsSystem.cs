using System;
using AYellowpaper.SerializedCollections;
using Kuhpik;
using Source.Scripts.Data.Enum;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Source.Scripts.System.Util
{
    public class CheatsSystem : GameSystem
    {
        [SerializeField] private SerializedDictionary<ElementType,Button> buttons;
        [SerializeField] private Button rndButton;

        public override void OnInit()
        {
            base.OnInit();
            foreach (var kv in buttons)
            {
                kv.Value.onClick.AddListener(() =>
                {
                    var ammo = pool.Ammo.Get(game.PlayerEntity);
                    var firstAmmo = ammo.Value[^ammo.Count];
                    pool.Element.Get(firstAmmo).Value = kv.Key;
                    pool.ModelChangerComponent.Get(firstAmmo).Value.SetModel((int)kv.Key);
                });
            }
            rndButton.onClick.AddListener(() =>
            {
                var ammo = pool.Ammo.Get(game.PlayerEntity);
                var firstAmmo = ammo.Value[^ammo.Count];
                var values = Enum.GetValues(typeof(ElementType));
                var value = (ElementType)values.GetValue(Random.Range(0, values.Length));
                pool.Element.Get(firstAmmo).Value = value;
                pool.ModelChangerComponent.Get(firstAmmo).Value.SetModel((int)value);
            });
            
        }
    }
}