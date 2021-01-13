using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace QuestLog
{
    public class Mod : IUserMod
    {
        #region Mod

        public string Name => "Questlog";

        public string Description => "Adds a handy questlog";

        public void OnEnabled()
        {
            Debug.Log((object)"Questlog enabled");
        }

        public void OnDisabled()
        {
            Debug.Log((object)"Questlog disabled");
        }
    }

    #endregion

    #region Create Questlog Panel

    public class TestBehaviour : MonoBehaviour
    {
        private void Start()
        {
            var uiView = UIView.GetAView();
            uiView.AddUIComponent(typeof(QuestLogPanel));
        }
    }

    #endregion

    #region Questlog panel

    public class QuestLogPanel : UIPanel
    {
        private List<StringHolder> currentHolders;

        private bool createdUI = false;

        public override void Start()
        {
            name = "Questlog";
            backgroundSprite = "GenericPanelDark";
            opacity = 0.5f;
            autoFitChildrenHorizontally = true;
            autoFitChildrenVertically = true;

            relativePosition = new Vector3(1200, 65, 0);
            zOrder = 0;

            autoLayoutDirection = LayoutDirection.Vertical;
            autoLayout = true;

            InvokeRepeating(nameof(GetHolders), 0.5f, 1f);
            InvokeRepeating(nameof(UpdateUI), 1f, 1f);


        }

        private void UpdateUI()
        {
            if (createdUI)
            {
                if (currentHolders == null || !currentHolders.Any()) return;
                var toBeDeleted = new List<StringHolder>();
                foreach (var holder in currentHolders)
                {
                    var oldLabelObject = Find("QuestLogLabel " + holder.ID);

                    if (oldLabelObject == null) continue;

                    var oldLabel = oldLabelObject.GetComponent<UILabel>();

                    if (oldLabel == null) continue;

                    if (holder.Passed || holder.MAX <= holder.Current)
                    {
                        RemoveUIComponent(oldLabel);
                        toBeDeleted.Add(holder);
                        Destroy(oldLabel.gameObject);
                    }

                    if (holder.Description.Contains("of electricity production from renewable sources"))
                    {
                        var oldBarObject = Find("QuestLogBar " + holder.ID);

                        if (oldBarObject == null) continue;

                        var oldBar = oldBarObject.GetComponent<UIProgressBar>();

                        if (oldBar == null) continue;

                        if (holder.Passed || holder.MAX <= holder.Current)
                        {
                            Debug.Log("wollah wat doe jij hier)");
                            RemoveUIComponent(oldBar);

                            if (currentHolders.Contains(holder) && !toBeDeleted.Contains(holder))
                            {
                                toBeDeleted.Add(holder);
                            }

                            Destroy(oldBar.gameObject);
                            continue;
                        }

                        oldBar.value = holder.Current;
                    }
                    else
                    {
                        if (oldLabel == null && oldLabel.text == null && holder.Progress == null && holder.Description == null) continue;  
                        oldLabel.text = holder.Description + "" + holder.Progress + "\n";                        
                    }
                }
                if (toBeDeleted == null || !toBeDeleted.Any() || !currentHolders.Any()) return;

                foreach (var item in toBeDeleted)
                {
                    currentHolders.Remove(item);
                }
                toBeDeleted.Clear();
            }
            else
            {
                if (currentHolders != null && currentHolders.Any())
                {
                    foreach (var holder in currentHolders.Where(holder => !holder.Passed))
                    {
                        HardCodedSwitchCase(holder);
                    }
                }
                createdUI = true;
            }
        }

        private void HardCodedSwitchCase(StringHolder holder)
        {
            switch (holder.Name)
            {
                //go to default unless one of the following.
                default:
                    CreateUI(holder);
                    break;
                case "Lose1":
                case "Start":
                case "Start1":
                case "Goal1":
                case "Punishment2":
                case "Coal_Repeatable":
                case "Oil_Repeatable":
                case "Lose-money":
                case "Lose-pop":
                case "Lose-pol":
                case "Lose-time":
                case "Disaster":
                case "Nuclear":
                case "Updraft1":
                case "SolarPanel3":
                case "SolarPanel1":
                case "Hydrogen":
                case "SolarPanel2":
                case "LargeSolar3":
                case "SmallHydrogen":
                case "LargeWind3":
                case "Wind 4":
                case null:
                    break;
            }
        }

        private void CreateUI(StringHolder holder)
        {
            var newLabel = AddUIComponent<UILabel>();
            newLabel.name = "QuestLogLabel " + holder.ID;
            newLabel.padding = new RectOffset(10, 10, 5, 5);
            newLabel.autoSize = true;
            newLabel.prefix = "- ";

            newLabel.text = holder.Description + "\n";

            if (holder.Description.Contains("of electricity production from renewable sources"))
            {
                var bar = AddUIComponent<UIProgressBar>();
                bar.name = "QuestLogBar " + holder.ID;
                bar.progressSprite = "ProgressBarFill";
                bar.backgroundSprite = "GenericPanelDark";
                bar.progressColor = new Color(0, 255, 0);
                bar.fillMode = UIFillMode.Fill;
                bar.minValue = holder.MIN;
                bar.maxValue = holder.MAX;
                bar.size = new Vector2(450, 20);
                bar.value = holder.Current;
            }
            else
            {
                newLabel.text = holder.Description + "" + holder.Progress + "\n";
            }
        }

        private void GetHolders()
        {
            var itemArray = new List<StringHolder>();
            var count = 0;

            if (Singleton<UnlockManager>.instance.m_scenarioTriggers != null)
            {
                foreach (var item in Singleton<UnlockManager>.instance.m_scenarioTriggers)
                {
                    if (item == null || item.m_conditions == null) continue;

                    foreach (var a in item.m_conditions)
                    {
                        if (a == null) continue;
                        var newHolder = new StringHolder(count, item.m_triggerName,
                            a.GetLocalizedProgress().m_description, a.GetLocalizedProgress().m_progress,
                            a.GetLocalizedProgress().m_min, a.GetLocalizedProgress().m_current,
                            a.GetLocalizedProgress().m_max,
                            a.GetLocalizedProgress().m_passed);
                        itemArray.Add(newHolder);
                        count++;
                    }
                }
            }
            currentHolders = itemArray;
        }
    }
}

#endregion

internal readonly struct StringHolder
{
    public readonly int ID;
    public readonly string Name;
    public readonly string Description;
    public readonly string Progress;
    public readonly float MIN;
    public readonly float Current;
    public readonly float MAX;
    public readonly bool Passed;

    public StringHolder(int id, string name, string description, string progress, float min, float current,
        float max, bool passed)
    {
        this.ID = id;
        this.Name = name ?? "";
        this.Description = description ?? "";
        this.Progress = progress ?? "";
        this.MIN = min;
        this.Current = current;
        this.MAX = max;
        this.Passed = passed;
    }

    public override string ToString()
    {
        return ID + " : " + Name + " : " + Description + " : " + Passed + " : "
               + Progress + " : " + MIN + " : " + Current + " : " + MAX + " : " + Passed;
    }
}
