
using UnityEngine;
using TMPro;
using AK.Wwise;

using System.Collections;
//using DG.Tweening;

public class DialogueAudio : MonoBehaviour
{
    //private VillagerScript villager;
    // private TMP_Animated animatedText;
    //public Transform mouthQuad;

    //public AudioClip[] voices;
    // public AudioClip[] punctuations;
    [Space]
    string current_dialogue=null;
    // public AudioSource voiceSource;
    //  public AudioSource punctuationSource;
  //  public AudioSource effectSource;
    public TextMeshProUGUI dialogue;
    string punctuation = ",.':;!? ";
    //[Space]
    // public AudioClip sparkleClip;
    // public AudioClip rainClip;

    // Start is called before the first frame update
    void Start()
    {
        // villager = GetComponent<VillagerScript>();

        // animatedText = InterfaceManager.instance.animatedText;

        // animatedText.onTextReveal.AddListener((newChar) => ReproduceSound(newChar));
    }

    void Update()
    {
     
            if (current_dialogue != dialogue.text)
            {
                StopAllCoroutines();

                StartCoroutine(ReproduceSound());

                current_dialogue = dialogue.text;
            Debug.Log("length:"+dialogue.text);
           
        }

        
    }
 


    IEnumerator ReproduceSound()
    {
        for(int i=0; i < dialogue.text.Length;)
        {
            char c = dialogue.text[i];
            if (!punctuation.Contains(c))
            {
                LetterSound(c);
                yield return new WaitForSeconds(0.03f);
            }
            else {
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
        AkSoundEngine.PostEvent("Play_" + letter, gameObject);

    }
}
  
    

