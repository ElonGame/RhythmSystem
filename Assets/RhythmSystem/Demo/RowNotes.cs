﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RowNotes : MonoBehaviour {
    public InputField tempo;
    public Slider swing;
    public AudioClip[] rowClips;
    private List<AudioSource> audioSources = new List<AudioSource>();

    void Start()
    {
        // Create an output audio source for each row
        for (int i = 0; i < rowClips.Length; i++)
        {
            GameObject go = new GameObject();
            go.name = rowClips[i].name + " Playback";
            go.transform.parent = transform;
            audioSources.Add(go.AddComponent<AudioSource>());
        }
        tempo.text = RhythmTracker.instance.GetTempo().ToString();
        tempo.onEndEdit.AddListener(delegate
        {
            TempoChange(tempo);
        }) ;
        
    }

    private void TempoChange(InputField input)
    {
        float newTempo;
        float.TryParse(input.text, out newTempo);
        RhythmTracker.instance.SetTempo(newTempo);
    }

    public void PlayRow(int row, int step)
    {
        // Pass it on to the swing coroutine
        StartCoroutine(PlaySwung(row, step));
    }

    private IEnumerator PlaySwung(int row, int step)
    {
        // If it's an even beat, delay based on the swing amount
        if (step % 2 != 0)
            yield return new WaitForSeconds(Mathf.Lerp(0, 15 / RhythmTracker.instance.GetTempo(), swing.value));

        audioSources[row].PlayOneShot(rowClips[row]);
    }
}
