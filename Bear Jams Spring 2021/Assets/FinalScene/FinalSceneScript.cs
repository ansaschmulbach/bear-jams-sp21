﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class FinalSceneScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tempTbox;

    private int currentLine = 0;
    private List<String> script;
    [SerializeField] private List<Vector3> positions;

    int mod(int x, int m) {
        return (x%m + m)%m;
    }

    void ShowNextLine()
    {
        if (currentLine < script.Count)
        {
            // Stop wobbling the old puppet.
            Debug.Log(GameManager.instance.gameState.numberOfSocks);
            if (GameManager.instance.gameState.numberOfSocks > 1)
            {
                GameManager.instance.gameState.socks[mod((currentLine - 1), GameManager.instance.gameState.socks.Count)]
                    .GetComponent<Animator>().Play("Nothing");
            }
            
            // Set the new text and start wobbling the new puppet.
            tempTbox.SetText(script[currentLine]);
            GameManager.instance.gameState.socks[mod(currentLine, GameManager.instance.gameState.socks.Count)]
                .GetComponent<Animator>().Play("PuppetWobble");
            
            // Increment line counter.
            currentLine += 1;
        }
        else
        {
            // Game's done.
            foreach (GameObject sock in GameManager.instance.gameState.socks)
            {
                Destroy(sock);
                GameManager.instance.Restart();
            }
            GameManager.instance.LoadMain();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        script = GameManager.instance.gameState.script;
        int i = 0;
        foreach (GameObject sock in GameManager.instance.gameState.socks)
        {
            //sock.GetComponent<SpriteRenderer>().enabled = true;
            foreach (SpriteRenderer sr in sock.GetComponentsInChildren<SpriteRenderer>()) {
                sr.enabled = true;
            }

            sock.transform.position = positions[i];
            sock.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            if (i == 1)
            {
                Vector3 curr = sock.transform.localScale;
                sock.transform.localScale = new Vector3(-curr.x, curr.y, curr.z);
            }
            i++;
        }

        ShowNextLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ShowNextLine();
        }
    }
}
