using UnityEngine;
using System.Reflection;
using HarmonyLib;
using ColossalFramework;

namespace QuestLog
{
    public static class Patcher
    {
        private const string HarmonyId = "haru.QuestLog";
        private static bool patched;

        public static void PatchAll()
        {
            if (patched)
            {
                Debug.Log("Questlog: already patched, Returning!");
                return;
            }
            Debug.Log("Questlog : Patching...");
            new Harmony(HarmonyId).PatchAll(Assembly.GetExecutingAssembly());
            patched = true;
        }

        public static void UnpatchAll()
        {
            if (!patched)
            {
                Debug.Log("Questlog: not yet patched, Returning!");
                return;
            }
            Debug.Log("Questlog: unPatching...");
            new Harmony(HarmonyId).UnpatchAll(HarmonyId);
            patched = false;
        }

        [HarmonyPatch(typeof(MilestoneCollection), "Awake")]
        public static class MilestoneCollectionManualMilestonesCaller
        {
            public static void Postfix()
            {
                //Debug.Log((object)"Reached Postfix!");
                //MilestoneInfo[] manualMilestones = MilestoneCollection.GetManualMilestones(ManualMilestone.Type.Create);
                //if (Singleton<UnlockManager>.instance.m_scenarioTriggers != null)//___m_Milestones != null && ___m_Milestones.Length > 0)
                //{
                //    string s = "";
                //    foreach (var item in Singleton<UnlockManager>.instance.m_scenarioTriggers) //___m_Milestones)
                //    {
                //        Debug.Log((object)"Name: " + item.GetLocalizedName());
                //        if (s == "")
                //        {
                //            s = item.GetLocalizedName();
                //        }
                //        else
                //        {
                //            s += "\n" + item.GetLocalizedName();
                //        }

                //        foreach (var a in item.m_conditions)
                //        {
                //            Debug.Log((object)"Name: " + a.m_name);

                //            s += "\n- Name: " + a.m_name + "\n - Description: " + a.GetLocalizedProgress().m_description +
                //                "\n - Progress String: " + a.GetLocalizedProgress().m_progress + "\n - Passed: " + a.GetLocalizedProgress().m_passed;
                //            s += "\n  CURRENT: " + a.GetLocalizedProgress().m_current + "\n  MIN: " +
                //                a.GetLocalizedProgress().m_min + "\n  MAX: " + a.GetLocalizedProgress().m_max;

                //        }
                //    }
                //    Debug.Log("Count: " + Singleton<UnlockManager>.instance.m_scenarioTriggers.Length);
                //    s += "\n\n\nCount: " + Singleton<UnlockManager>.instance.m_scenarioTriggers.Length;

                //    System.IO.File.WriteAllText(@"C:\Milestones.txt", s);

                //}
            }
        }
    }
}
