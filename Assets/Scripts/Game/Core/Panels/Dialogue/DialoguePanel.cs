using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game.Core.Panels
{
    public class DialoguePanel : MonoBehaviour
    {
        [SerializeField] private GameObject _context;
        
        [SerializeField] private TMP_Text _speakerText;
        [SerializeField] private TMP_Text _mainText;
        
        public void ShowPanel()
        {
            _context.SetActive(true);
        }
        
        public void HidePanel()
        {
            _context.SetActive(false);
        }
        
        [Button(ButtonStyle.FoldoutButton)]
        public UniTask ShowText(string speakerName, string text)
        {
            if (string.IsNullOrEmpty(speakerName))
            {
                _speakerText.gameObject.SetActive(false);
            }
            else
            {
                _speakerText.gameObject.SetActive(true);
                _speakerText.text = speakerName;
            }
            
            _mainText.text = text;

            return UniTask.CompletedTask;
        }
    }
}