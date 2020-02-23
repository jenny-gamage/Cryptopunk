﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class shop : MonoBehaviour
{
    [SerializeField] int inventorySize =3;
    internal List<GameObject> inventory;
    [SerializeField] GameObject[] shopItemAnchors;
    [SerializeField] GameObject shopItemPrefab;
    [SerializeField] Text playerCredits;

    [SerializeField] Color overBudgetColor = Color.red;
    private Color normalCreditsColor;
    [SerializeField] float overBudgetFlashInterval = 0.7f;
    private float overBudgetFlashTimer=0f;
    // Start is called before the first frame update
    void Start()
    {
        LoadInventory();
        normalCreditsColor = playerCredits.color;
    }

    private void Update()
    {
        playerCredits.text = "Credits: " + PersistentState.instance.credits;
        if(overBudgetFlashTimer>0)
        {
            overBudgetFlashTimer -= Time.deltaTime;
            playerCredits.color = overBudgetColor;
        }
        else
        {
            playerCredits.color = normalCreditsColor;
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene("desktop");
    }

    private void LoadInventory()
    {
        List<GameObject> buyableSchema = new List<GameObject>();
        inventory = new List<GameObject>();
        foreach(GameObject schema in PersistentState.instance.schemaLibrary)
        {
            if (!PersistentState.instance.GetOwnedPrograms().Contains(schema))
            {
                buyableSchema.Add(schema);
            }
        }

        while(buyableSchema.Count>0&&inventory.Count<inventorySize)
        {
            GameObject schema = buyableSchema[Random.Range(0, buyableSchema.Count)];
            inventory.Add(CreateInventoryItem(schema));
            buyableSchema.Remove(schema);
        }
    }

    private GameObject CreateInventoryItem(GameObject schema)
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        InventoryItem newItem = Instantiate(shopItemPrefab, canvas.transform).GetComponent<InventoryItem>();
        newItem.gameObject.transform.position = shopItemAnchors[inventory.Count].transform.position; 
        newItem.SetSchema(schema);
        return newItem.gameObject;
    }

    internal void Buy(InventoryItem inventoryItem)
    {
        PersistentState.instance.AddProgram(inventoryItem.item);
        PersistentState.instance.credits -= inventoryItem.cost;
        inventory.Remove(inventoryItem.gameObject);
        Destroy(inventoryItem.gameObject);
    }
    internal void OverBudgetFeedback()
    {
        overBudgetFlashTimer = overBudgetFlashInterval;
        Debug.Log("Not enough minerals");
    }
}
