using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpScreen : MonoBehaviour
{
    [SerializeField] private Transform options;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private ArsenalController arsenalController;
    [SerializeField] private UIController uIController;
    public WeaponLevel[] activeCards = new WeaponLevel[3];
    private void Start()
    {
        SetUpLevelScreen();
    }
    public void SetUpLevelScreen()
    {
        ClearCards();
        List<WeaponLevel> possibleUpgrades = arsenalController.PossibleUpgrades();
        if (possibleUpgrades != null)
        {
            int cards = 0;
            while(possibleUpgrades.Count > 0 && cards < 3)
            {
                int randomCardId = Random.Range(0, possibleUpgrades.Count);
                SpawnCard(possibleUpgrades[randomCardId]);
                possibleUpgrades.RemoveAt(randomCardId);
                cards++;
            }
        }
        else
        {
            SpawnCard();
        }
    }
    void SpawnCard(WeaponLevel weaponLevel)
    {
        GameObject card = Instantiate(cardPrefab, options);

        TMP_Text[] texts = card.GetComponentsInChildren<TMP_Text>();
        texts[0].text = weaponLevel.title;
        texts[1].text = weaponLevel.description;

        Image[] images = card.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            if (image.gameObject.name == "cardImage")
            {
                image.sprite = weaponLevel.image;
                image.SetNativeSize();
                break;
            }
        }

        Button button = card.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => arsenalController.UpgradeWeaponWithId(weaponLevel.weaponId, weaponLevel));
            button.onClick.AddListener(() => uIController.HideLevelUp());
            SaveCardsToChoose(weaponLevel);
            button.Select();
        }
    }

    void SpawnCard()
    {
        GameObject card = Instantiate(cardPrefab, options);
        Button button = card.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => uIController.HideLevelUp());

        }
    }
    void ClearCards()
    {
        foreach (Transform child in options.transform)
        {
            Destroy(child.gameObject);
        }
        activeCards = new WeaponLevel[3];
    }
    void SaveCardsToChoose(WeaponLevel weaponLevel)
    {
        for (int i = 0; i < activeCards.Length; i++)
        {
            activeCards[i] = weaponLevel;
        }
    }
}
