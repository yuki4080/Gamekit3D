using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Gamekit3D
{
    public class DialogueCanvasController : MonoBehaviour
    {
        public Animator animator;
        public TextMeshProUGUI textMeshProUGUI;
        public LocalizedStringTable stringTable;

        protected Coroutine m_DeactivationCoroutine;
    
        protected readonly int m_HashActivePara = Animator.StringToHash ("Active");

        IEnumerator SetAnimatorParameterWithDelay (float delay)
        {
            yield return new WaitForSeconds (delay);
            animator.SetBool(m_HashActivePara, false);
        }

        public void ActivateCanvasWithText (string text)
        {
            if (m_DeactivationCoroutine != null)
            {
                StopCoroutine (m_DeactivationCoroutine);
                m_DeactivationCoroutine = null;
            }

            gameObject.SetActive (true);
            animator.SetBool (m_HashActivePara, true);
            textMeshProUGUI.text = text;
        }

        public void ActivateCanvasWithTranslatedText (string phraseKey)
        {
            if (m_DeactivationCoroutine != null)
            {
                StopCoroutine(m_DeactivationCoroutine);
                m_DeactivationCoroutine = null;
            }

            gameObject.SetActive(true);
            animator.SetBool(m_HashActivePara, true);
            textMeshProUGUI.text = LocalizationSettings.StringDatabase.GetLocalizedString(stringTable.TableReference, phraseKey);
        }

        public void DeactivateCanvasWithDelay (float delay)
        {
            m_DeactivationCoroutine = StartCoroutine (SetAnimatorParameterWithDelay (delay));
        }
    }
}
