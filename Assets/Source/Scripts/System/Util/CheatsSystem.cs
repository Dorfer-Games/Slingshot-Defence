using System;
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
        [SerializeField] private Button toggle;
        [SerializeField] private GameObject cheatsUI;
        [SerializeField] private Button lvlup;
        [SerializeField] private Button nextStage;
        [SerializeField] private SerializedDictionary<ElementType, Button> buttons;

        [SerializeField] private SerializedDictionary<TomeType, Button> tomeButtons;
        //[SerializeField] private Button rndButton;
#if UNITY_EDITOR
        public override void OnInit()
        {
            base.OnInit();
            toggle.onClick.AddListener(()=>
            {
                cheatsUI.gameObject.SetActive(!cheatsUI.activeSelf);
            });
            lvlup.onClick.AddListener(()=>
            {
                pool.LvlUpEvent.Add(eventWorld.NewEntity());
            });
            
            nextStage.onClick.AddListener(()=>
            {
                pool.NextStageEvent.Add(eventWorld.NewEntity());
            });
            
            foreach (var kv in tomeButtons)
            {
                kv.Value.onClick.AddListener(() =>
                {
                    if (pool.Tomes.Get(game.PlayerEntity).Value[kv.Key] < config.ThroughTome.Length-1)
                    {
                        pool.Tomes.Get(game.PlayerEntity).Value[kv.Key] += 1;
                        kv.Value.GetComponentInChildren<TextMeshProUGUI>().text +=
                            "\n" + pool.Tomes.Get(game.PlayerEntity).Value[kv.Key].ToString();
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
                    ref var level = ref pool.Level.Get(firstAmmo).Value;
                    level++;
                    int modelID = (int) kv.Key;
                    if (level > 4)
                    {
                        level = 5;
                        modelID = config.ElementsCount - 1 + (int) kv.Key;
                        pool.Ult.GetOrCreateRef(firstAmmo).Value = (UltType) kv.Key;
                    }
                    kv.Value.GetComponentInChildren<TextMeshProUGUI>().text +=
                        "\n" + level;
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