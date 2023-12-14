using System;
using System.Collections.Generic;
using System.Linq;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Event;
using Source.Scripts.Data.Enum;
using Source.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.System.Loot
{
    public class LvlUpSystem : GameSystemWithScreen<LvlUpUIScreen>
    {
        private EcsFilter filter;
        private int upsCount;

        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<LvlUpEvent>().End();

            foreach (var upCardView in screen.UpCards)
            {
                upCardView.Init(config.UIConfig);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var ent in filter)
            {
                if (upsCount == 0 && game.Joystick.Direction.Equals(Vector2.zero))
                {
                    SetScreen();
                }

                upsCount++;
            }
        }

        #region Upgrades

        private void UpgradeTome(TomeType tomeType)
        {
            var tomes = pool.Tomes.Get(game.PlayerEntity).Value;
            tomes[tomeType]++;
        }

        private void UpgradeBall(ElementType elementType)
        {
            var ammoBalls = pool.Ammo.Get(game.PlayerEntity).Value;

            //if has same 
            foreach (var ammoBall in ammoBalls)
            {
                if (pool.Element.Get(ammoBall).Value == elementType)
                {
                    ref var level = ref pool.Level.Get(ammoBall).Value;
                    level++;
                    int modelID = (int) elementType;
                    if (level > 4)
                    {
                        modelID = config.ElementsCount - 1 + (int) elementType;
                        pool.Ult.GetOrCreateRef(ammoBall).Value = (UltType) elementType;
                    }

                    pool.ModelChangerComponent.Get(ammoBall).Value.SetModel(modelID);
                    return;
                }
            }


            //if doest have same element - finding active views
            foreach (var ammoBall in ammoBalls)
            {
                ref var el = ref pool.Element.Get(ammoBall).Value;
                if (el == ElementType.DEFAULT)
                {
                    if (pool.View.Get(ammoBall).Value.gameObject.activeSelf)
                    {
                        el = elementType;
                        pool.Level.Get(ammoBall).Value++;
                        pool.ModelChangerComponent.Get(ammoBall).Value.SetModel((int) elementType);
                        return;
                    }
                }
            }

            //if doest have same element - finding nonactive views
            foreach (var ammoBall in ammoBalls)
            {
                ref var el = ref pool.Element.Get(ammoBall).Value;
                if (el == ElementType.DEFAULT)
                {
                    el = elementType;
                    pool.Level.Get(ammoBall).Value++;
                    pool.ModelChangerComponent.Get(ammoBall).Value.SetModel((int) elementType);
                    return;
                }
            }
        }

        #endregion

        #region UI

        private void SetScreen()
        {
            screen.Open();
            Time.timeScale = 0;
            game.Joystick.OnPointerUp(null);

            ConsiderUps(out List<ElementType> resElements, out List<ElementType> resUlts, out List<TomeType> resTomes);
            SetUpgradesUI(resElements, resUlts, resTomes);
        }

        private void OnUpsEnd()
        {
            Time.timeScale = 1;
            screen.Close();
            upsCount--;
            if (upsCount > 0)
            {
                SetScreen();
            }
        }

        private void SetUpgradesUI(List<ElementType> resElements, List<ElementType> resUlts, List<TomeType> resTomes)
        {
            var screenUpCards = screen.UpCards.ToList();
            Shuffle(screenUpCards);

            foreach (var elementType in resUlts)
            {
                var screenUpCard = screenUpCards[0];
                screenUpCard.SetUlt(elementType);
                screenUpCard.Button.onClick.RemoveAllListeners();
                screenUpCard.Button.onClick.AddListener(() =>
                {
                    UpgradeBall(elementType);
                    OnUpsEnd();
                });
                screenUpCards.RemoveAt(0);
            }

            foreach (var elementType in resElements)
            {
                var screenUpCard = screenUpCards[0];
                screenUpCard.SetBall(elementType, GetBallLevelByType(elementType));
                screenUpCard.Button.onClick.RemoveAllListeners();
                screenUpCard.Button.onClick.AddListener(() =>
                {
                    UpgradeBall(elementType);
                    OnUpsEnd();
                });
                screenUpCards.RemoveAt(0);
            }

            foreach (var tomeType in resTomes)
            {
                var screenUpCard = screenUpCards[0];
                screenUpCard.SetTome(tomeType, GetTomeLevelByType(tomeType));
                screenUpCard.Button.onClick.RemoveAllListeners();
                screenUpCard.Button.onClick.AddListener(() =>
                {
                    UpgradeTome(tomeType);
                    OnUpsEnd();
                });
                screenUpCards.RemoveAt(0);
            }
        }

        #endregion

        #region ConsiderUps

        private void ConsiderUps(out List<ElementType> resElements, out List<ElementType> resUlts,
            out List<TomeType> resTomes)
        {
            resElements = new();
            resUlts = new();
            resTomes = new();
            List<ElementType> allElements = GetAllElements();
            List<TomeType> allTomes = GetAllTomes();

            RemoveMaxes(allElements, allTomes);
            AddUltUpgrades(allElements, resUlts, resTomes); //add to roll ults and tomes to ult
            TryFillWithRepeating(allElements, allTomes, resElements,
                resTomes); //add to roll repeating balls and tomes with chance

            while (resElements.Count + resTomes.Count + resUlts.Count < 3)
            {
                var ballTomeRnd = Random.Range(0, 2); // 0 - ball || 1 - tome
                if (ballTomeRnd == 0)
                {
                    resElements.Add(allElements[Random.Range(0, allElements.Count)]);
                }
                else
                {
                    resTomes.Add(allTomes[Random.Range(0, allElements.Count)]);
                }
            }

            TrimResult(ref resElements, ref resUlts, ref resTomes);
        }

        private void TrimResult(ref List<ElementType> resElements, ref List<ElementType> resUlts,
            ref List<TomeType> resTomes)
        {
            if (resUlts.Count > 3)
            {
                resUlts = resUlts.GetRange(0, 3);
                resElements.Clear();
                resTomes.Clear();
            }
            else
            {
                var residue = 3 - resUlts.Count;
                if (resElements.Count > residue)
                {
                    resElements = resElements.GetRange(0, residue);
                    resTomes.Clear();
                }
                else
                {
                    var residue1 = residue - resElements.Count;
                    if (resTomes.Count > residue1)
                    {
                        resTomes = resTomes.GetRange(0, residue1);
                    }
                }
            }
        }

        private void RemoveMaxes(List<ElementType> allElements, List<TomeType> allTomes)
        {
            var ammoBalls = pool.Ammo.Get(game.PlayerEntity).Value;
            foreach (var ammoBall in ammoBalls)
            {
                var lvl = pool.Level.Get(ammoBall).Value;
                if (lvl > 4)
                {
                    var elementType = pool.Element.Get(ammoBall).Value;
                    allElements.Remove(elementType);
                }
            }

            var tomes = pool.Tomes.Get(game.PlayerEntity).Value;
            foreach (var kv in tomes)
            {
                if (kv.Value == config.ThroughTome.Length - 1)
                {
                    allTomes.Remove(kv.Key);
                }
            }
        }

        private void AddUltUpgrades(List<ElementType> allElements, List<ElementType> resUlts,
            List<TomeType> resTomes)
        {
            var tomes = pool.Tomes.Get(game.PlayerEntity).Value;
            var ammoBalls = pool.Ammo.Get(game.PlayerEntity).Value;
            foreach (var ammoBall in ammoBalls)
            {
                var lvl = pool.Level.Get(ammoBall).Value;
                if (lvl == 4)
                {
                    var elementType = pool.Element.Get(ammoBall).Value;
                    //has ball level and tome level
                    if (tomes[(TomeType) elementType - 1] > 0)
                    {
                        allElements.Remove(elementType);
                        resUlts.Add(elementType);
                    }
                    else //need tome
                    {
                        allElements.Remove(elementType);
                        resTomes.Add((TomeType) elementType - 1);
                    }
                }
            }
        }

        private void TryFillWithRepeating(List<ElementType> allElements, List<TomeType> allTomes,
            List<ElementType> resElements, List<TomeType> resTomes)
        {
            var tomes = pool.Tomes.Get(game.PlayerEntity).Value;
            var ammoBalls = pool.Ammo.Get(game.PlayerEntity).Value;

            var sameBallChance = Random.Range(1, 101);
            var sameTomeChance = Random.Range(1, 101);

            if (sameBallChance <= config.SameBallInRollChancePercent)
            {
                var elementType = GetSameBall(ammoBalls, allElements);
                if (elementType != ElementType.DEFAULT)
                    resElements.Add(elementType);
            }

            if (sameTomeChance <= config.SameTomeInRollChancePercent)
            {
                var tomeType = GetSameTome(tomes, allTomes);
                resTomes.Add(tomeType);
            }
        }

        private List<TomeType> GetAllTomes()
        {
            List<TomeType> res = new();
            var values = Enum.GetValues(typeof(TomeType));
            foreach (var value in values)
                res.Add((TomeType) value);

            return res;
        }

        private List<ElementType> GetAllElements()
        {
            List<ElementType> res = new();
            var values = Enum.GetValues(typeof(ElementType));
            foreach (var value in values)
                res.Add((ElementType) value);
            res.RemoveAt(0);
            return res;
        }

        private TomeType GetSameTome(Dictionary<TomeType, int> tomes, List<TomeType> allTomes)
        {
            List<List<TomeType>> typesByLevels = new();
            for (int i = 0; i < config.ThroughTome.Length; i++)
                typesByLevels.Add(new List<TomeType>());

            foreach (var kv in tomes)
            {
                var lvl = kv.Value;
                if (allTomes.Contains(kv.Key))
                    typesByLevels[lvl].Add(kv.Key);
            }

            for (int i = typesByLevels.Count - 1; i >= 0; i--)
            {
                if (typesByLevels[i].Count == 0)
                    continue;
                //randomise tomes with equal level
                return typesByLevels[i][Random.Range(0, typesByLevels[i].Count)];
            }

            return TomeType.THROUGH;
        }

        private ElementType GetSameBall(List<int> balls, List<ElementType> allElements)
        {
            List<List<ElementType>> elByLevels = new();
            for (int i = 0; i <= config.MaxSimpleBallLevel; i++)
                elByLevels.Add(new List<ElementType>());

            foreach (var ent in balls)
            {
                var lvl = pool.Level.Get(ent).Value;
                var elementType = pool.Element.Get(ent).Value;
                if (allElements.Contains(elementType))
                    elByLevels[lvl].Add(elementType);
            }

            for (int i = elByLevels.Count - 1; i >= 0; i--)
            {
                if (elByLevels[i].Count == 0)
                    continue;
                //randomise balls with equal level
                return elByLevels[i][Random.Range(0, elByLevels[i].Count)];
            }

            return ElementType.DEFAULT;
        }

        #endregion

        #region Util

        private int GetTomeLevelByType(TomeType tomeType)
        {
            var tomes = pool.Tomes.Get(game.PlayerEntity).Value;
            return tomes[tomeType];
        }

        private int GetBallLevelByType(ElementType elementType)
        {
            var ammoBalls = pool.Ammo.Get(game.PlayerEntity).Value;
            foreach (var ammoBall in ammoBalls)
            {
                if (pool.Element.Get(ammoBall).Value == elementType)
                {
                    return pool.Level.Get(ammoBall).Value;
                }
            }

            return 0;
        }

        private void Shuffle(List<UpCardView> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        #endregion
    }
}