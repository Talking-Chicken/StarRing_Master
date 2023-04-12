using UnityEngine;
using TMPro;
using AK.Wwise;
using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using System.Collections;
using MoreMountains.Tools;
//using DG.Tweening;


[System.Serializable]

public class FacialExpression
{
    public string char_name;
    public string gameobject_name;
    public Animator char_anime;
}


public class DialogueAudio : MMSingleton<DialogueAudio>
{
    //private VillagerScript villager;
    // private TMP_Animated animatedText;
    //public Transform mouthQuad;

    //public AudioClip[] voices;
    // public AudioClip[] punctuations;
    [Space]
    string current_dialogue = null;
    // Animator animator;
    // public AudioSource voiceSource;
    //  public AudioSource punctuationSource;
    //  public AudioSource effectSource;
    public TextMeshProUGUI dialogue;
    public TextMeshProUGUI character_name;
    string current_character = null;
    string punctuation = ",.':;!? ...";
    bool run_once = false;
    Animator last_anime;
    //public Animator[] facial_expression;
    [SerializeField]
    private List<FacialExpression> facial_bank;
    public MMF_Player zoomIn;
    public MMF_Player zoomOut;
    //[Space]
    // public AudioClip sparkleClip;
    // public AudioClip rainClip;

    // Start is called before the first frame update
    void Start()
    {
        // villager = GetComponent<VillagerScript>();

        // animatedText = InterfaceManager.instance.animatedText;

        // animatedText.onTextReveal.AddListener((newChar) => ReproduceSound(newChar));
        foreach (FacialExpression facialExpression in facial_bank)
        {
            facialExpression.char_anime = GameObject.Find(facialExpression.gameobject_name).transform.Find("Face").GetComponent<Animator>();
        }
        last_anime = null;
    }

    void Update()
    {
        if (current_dialogue != dialogue.text)
        {
            StopAllCoroutines();

            StartCoroutine(ReproduceSound());

            current_dialogue = dialogue.text;
            Debug.Log("length:" + dialogue.text);
        }

        if (current_character != character_name.text)
        {
            run_once = true;


            current_character = character_name.text;
        }

        if (run_once)
        {
            Debug.Log("test");
            if (last_anime != null)
            {
                last_anime.Play("stop_talking");
            }


            run_once = false;



            PlayAnimation(character_name.text);
        }




    }



    IEnumerator ReproduceSound()
    {
        int dialogue_length = dialogue.text.Length / 2;
        for (int i = 0; i < dialogue_length;)
        {
            char c = dialogue.text[i];
            if (!punctuation.Contains(c))
            {
                LetterSound(c);
                yield return new WaitForSeconds(0.03f);
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }

            i++;
        }


        /*
        if (char.IsPunctuation(c) && !punctuationSource.isPlaying)
        {
            voiceSource.Stop();
            punctuationSource.clip = punctuations[Random.Range(0, punctuations.Length)];
            punctuationSource.Play();
        }

        if (char.IsLetter(c) && !voiceSource.isPlaying)
        {
            punctuationSource.Stop();
            voiceSource.clip = voices[Random.Range(0, voices.Length)];
            voiceSource.Play();

            LetterSound(c);


            mouthQuad.localScale = new Vector3(1, 0, 1);
           // mouthQuad.DOScaleY(1, .2f).OnComplete(() => mouthQuad.DOScaleY(0, .2f));
        }
        */
    }
    public void LetterSound(char letter)
    {
        letter = char.ToUpper(letter);
        AkSoundEngine.PostEvent("Play_" + letter + "_" + character_name.text, gameObject);

    }
    public void ZoomOut() { zoomOut.PlayFeedbacks(); }
    public void ZoomIn() { zoomIn.PlayFeedbacks(); }
    public AnimationClip PlayAnimation(string character_name)
    {
        foreach (FacialExpression facialExpression in facial_bank)
        {
            if (facialExpression.char_name == character_name)
            {
                facialExpression.char_anime.Play("talking");
                last_anime = facialExpression.char_anime;
            }
        }
        return null;
    }
}



