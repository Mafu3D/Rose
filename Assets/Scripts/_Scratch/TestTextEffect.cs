using System.Collections;
using EasyTextEffects;
using TMPro;
using UnityEngine;

public class TestTextEffect : MonoBehaviour
{
    [SerializeField] TMP_Text testText;
    private bool spacePressed = false;
    private bool fPressed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !spacePressed)
        {
            spacePressed = true;
            StartCoroutine(DamageTextRoutine());

        }

        if (Input.GetKeyDown(KeyCode.F) && !fPressed)
        {
            fPressed = true;
            StartCoroutine(HealTextRoutine());

        }
    }

    private IEnumerator DamageTextRoutine()
    {
        TextEffect textEffect = testText.GetComponent<TextEffect>();
        if (testText != null)
        {
            textEffect.StartManualEffect("damage");
            textEffect.StartManualEffect("changed");
        }
        yield return new WaitForSecondsRealtime(0.2f);
        if (testText != null)
        {
            textEffect.StopManualEffects();
        }
        spacePressed = false;
    }

    private IEnumerator HealTextRoutine()
    {
        TextEffect textEffect = testText.GetComponent<TextEffect>();
        if (testText != null)
        {
            textEffect.StartManualEffect("heal");
            textEffect.StartManualEffect("changed");
        }
        yield return new WaitForSecondsRealtime(0.2f);
        if (testText != null)
        {
            textEffect.StopManualEffects();
        }
        fPressed = false;
    }
}
