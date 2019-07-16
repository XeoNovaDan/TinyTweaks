using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorld.Planet;

namespace TinyTweaks
{

    public class Dialog_AssignCaravanFoodRestrictions : Window
    {

        public Dialog_AssignCaravanFoodRestrictions(Caravan caravan)
        {
            // I totally didn't just copy-paste Dialog_AssignCaravanDrugPolicies and adapt it
            this.caravan = caravan;
            this.doCloseButton = true;
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(550f, 500f);
            }
        }

        public override void DoWindowContents(Rect rect)
        {
            rect.height -= this.CloseButSize.y;
            float num = 0f;
            Rect rect2 = new Rect(rect.width - 354f - 16f, num, 354f, 32f);
            if (Widgets.ButtonText(rect2, "ManageFoodRestrictions".Translate(), true, false, true))
            {
                Find.WindowStack.Add(new Dialog_ManageFoodRestrictions(null));
            }
            num += 42f;
            Rect outRect = new Rect(0f, num, rect.width, rect.height - num);
            Rect viewRect = new Rect(0f, 0f, rect.width - 16f, this.lastHeight);
            Widgets.BeginScrollView(outRect, ref this.scrollPos, viewRect, true);
            float num2 = 0f;
            for (int i = 0; i < this.caravan.pawns.Count; i++)
            {
                if (this.caravan.pawns[i].foodRestriction != null)
                {
                    if (num2 + 30f >= this.scrollPos.y && num2 <= this.scrollPos.y + outRect.height)
                    {
                        this.DoRow(new Rect(0f, num2, viewRect.width, 30f), this.caravan.pawns[i]);
                    }
                    num2 += 30f;
                }
            }
            this.lastHeight = num2;
            Widgets.EndScrollView();
        }

        private void DoRow(Rect rect, Pawn pawn)
        {
            Rect rect2 = new Rect(rect.x, rect.y, rect.width - 354f, 30f);
            Text.Anchor = TextAnchor.MiddleLeft;
            Text.WordWrap = false;
            Widgets.Label(rect2, pawn.LabelCap);
            Text.Anchor = TextAnchor.UpperLeft;
            Text.WordWrap = true;
            GUI.color = Color.white;
            Rect rect3 = new Rect(rect.x + rect.width - 354f, rect.y, 354f, 30f);
            DoAssignFoodRestrictionButtons(rect3, pawn);
        }

        private void DoAssignFoodRestrictionButtons(Rect rect, Pawn pawn)
        {
            int num = Mathf.FloorToInt((rect.width - 4f) * 0.714285731f);
            int num2 = Mathf.FloorToInt((rect.width - 4f) * 0.2857143f);
            float num3 = rect.x;
            Rect rect2 = new Rect(num3, rect.y + 2f, (float)num, rect.height - 4f);
            Rect rect3 = rect2;
            Func<Pawn, FoodRestriction> getPayload = (Pawn p) => p.foodRestriction.CurrentFoodRestriction;
            Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<FoodRestriction>>> menuGenerator = new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<FoodRestriction>>>(this.Button_GenerateMenu);
            string buttonLabel = pawn.foodRestriction.CurrentFoodRestriction.label.Truncate(rect2.width, null);
            string label = pawn.foodRestriction.CurrentFoodRestriction.label;
            Widgets.Dropdown<Pawn, FoodRestriction>(rect3, pawn, getPayload, menuGenerator, buttonLabel, null, label, null, null, true);
            num3 += (float)num;
            num3 += 4f;
            Rect rect4 = new Rect(num3, rect.y + 2f, (float)num2, rect.height - 4f);
            if (Widgets.ButtonText(rect4, "AssignTabEdit".Translate(), true, false, true))
            {
                Find.WindowStack.Add(new Dialog_ManageFoodRestrictions(pawn.foodRestriction.CurrentFoodRestriction));
            }
            num3 += (float)num2;
        }

        private IEnumerable<Widgets.DropdownMenuElement<FoodRestriction>> Button_GenerateMenu(Pawn pawn)
        {
            using (List<FoodRestriction>.Enumerator enumerator = Current.Game.foodRestrictionDatabase.AllFoodRestrictions.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    FoodRestriction foodRestriction = enumerator.Current;
                    yield return new Widgets.DropdownMenuElement<FoodRestriction>
                    {
                        option = new FloatMenuOption(foodRestriction.label, delegate ()
                        {
                            pawn.foodRestriction.CurrentFoodRestriction = foodRestriction;
                        }, MenuOptionPriority.Default, null, null, 0f, null, null),
                        payload = foodRestriction
                    };
                }
            }
            yield break;
        }

        private Caravan caravan;

        private Vector2 scrollPos;

        private float lastHeight;

        private const float RowHeight = 30f;

        private const float AssignDrugPolicyButtonsTotalWidth = 354f;

        private const int ManageDrugPoliciesButtonHeight = 32;

    }

}
