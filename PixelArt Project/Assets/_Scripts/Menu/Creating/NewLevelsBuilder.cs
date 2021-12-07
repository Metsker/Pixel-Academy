using UnityEngine;

namespace _Scripts.Menu.Creating
{
    public class NewLevelsBuilder : CategoryBuilder
    {
        [SerializeField] private GameObject panel;
        private new void Start()
        {
            base.Start();
            for (var i = 0; i < group.levels.Count; i++)
            {
                var g = Instantiate(creatingData.categoryInstance, panel.transform);
                g.name = "New Level " + i;
            }
            LoadChildren(panel);
        }
    }
}