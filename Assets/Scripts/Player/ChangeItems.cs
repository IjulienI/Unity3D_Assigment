using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeItems : MonoBehaviour
{
    [SerializeField] List<GameObject> items;
    private int index;

    private void Start()
    {
        UpdateItem();
    }
    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            index++;
            UpdateItem();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            index--;
            UpdateItem();
        }
    }

    private void UpdateItem()
    {
        if (index > items.Count -1)
        {
            index = 0;
        }
        if (index < 0)
        {
            index = items.Count -1;
        }

        foreach (GameObject item in items)
        {
            item.SetActive(false);
        }
        items[index].SetActive(true);
    }

    public void AddItem(GameObject item)
    {
        items.Add(item);
        index--;
    }

    public void RemoveItem(GameObject item)
    {
        items.Remove(item);
        Destroy(item);
        UpdateItem();
    }
}
