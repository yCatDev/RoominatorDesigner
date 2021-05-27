using UnityEngine;

namespace Logic.Designer
{
    public class Searcher: MonoBehaviour
    {
        [SerializeField] private SearchEntry[] entryes;

        public void Search(string text)
        {
            foreach (var searchEntry in entryes)
            {
                foreach (var entryName in searchEntry.Names)
                {
                    if (!entryName.ToLower().Contains(text.ToLower()))
                        searchEntry.gameObject.SetActive(false);
                    else
                        searchEntry.gameObject.SetActive(true);
                }
            }
        }
        
    }
}