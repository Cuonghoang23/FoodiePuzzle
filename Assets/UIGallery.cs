using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGallery : UIBase
{
    private ItemDatabase itemDatabase;

    [Header("UI")]
    public GameObject itemPrefab;
    public Transform content;

    [Header("Page")]
    public int maxItemPerPage = 12;
    public Button btnNext;
    public Button btnPrev;

    private List<Image> galleryItems = new List<Image>();

    private int currentPage = 0;
    private int totalPage = 0;

    private bool isInit = false;

    void Start()
    {
        InitGallery();
    }

    protected override void OnEnable()
    {

        InitGallery();
        ShowPage(currentPage);
        base.OnEnable();
    }

    void InitGallery()
    {
        if (isInit) return;
        isInit = true;

        itemDatabase = GameData.Instance.ItemDatabase;

        totalPage = Mathf.CeilToInt((float)itemDatabase.items.Count / maxItemPerPage);

        // Chỉ spawn tối đa số item của 1 trang
        for (int i = 0; i < maxItemPerPage; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, content);
            Image img = newItem.GetComponent<Image>();

            galleryItems.Add(img);
        }

        if (btnNext != null)
            btnNext.onClick.AddListener(NextPage);

        if (btnPrev != null)
            btnPrev.onClick.AddListener(PrevPage);
    }

    void ShowPage(int page)
    {
        currentPage = page;

        int startIndex = currentPage * maxItemPerPage;

        for (int i = 0; i < galleryItems.Count; i++)
        {
            int itemIndex = startIndex + i;

            Image img = galleryItems[i];

            if (itemIndex < itemDatabase.items.Count)
            {
                img.gameObject.SetActive(true);

                img.sprite = itemDatabase.items[itemIndex].icon;

                if (itemDatabase.items[itemIndex].levelUnlock < GameData.Instance.CurrentLevel)
                {
                    img.color = Color.white;
                }
                else
                {
                    img.color = Color.black;
                }
            }
            else
            {
                img.gameObject.SetActive(false);
            }
        }

        UpdateButtonPage();
    }

    public void NextPage()
    {
        if (currentPage >= totalPage - 1) return;

        currentPage++;
        ShowPage(currentPage);
    }

    public void PrevPage()
    {
        if (currentPage <= 0) return;

        currentPage--;
        ShowPage(currentPage);
    }

    void UpdateButtonPage()
    {
        if (btnNext != null)
            btnNext.gameObject.SetActive(currentPage < totalPage - 1);

        if (btnPrev != null)
            btnPrev.gameObject.SetActive(currentPage > 0);
    }
}