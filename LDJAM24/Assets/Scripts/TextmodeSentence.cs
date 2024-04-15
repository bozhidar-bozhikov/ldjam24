using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextmodeSentence : MonoBehaviour
{
    public RectTransform rectTransform;
    public GridLayoutGroup grid;

    public List<Sprite> symbols;
    public GameObject symbolPrefab;

    public List<Color> colors;

    private void Start()
    {

    }

    public void DisplayString(string sentence)
    {
        StartCoroutine(_DisplayString(sentence, 0));
    }

    public void DisplayString(string sentence, int frameCount)
    {
        StartCoroutine(_DisplayString(sentence, frameCount));
    }

    private IEnumerator _DisplayString(string sentence, int frameCount) //You dealt <r41(6d6) fire> damage to the <ggoblin>!
    {
        Clear();

        if (sentence == null)
        {
            print("Empty sentence!");
            yield break;
        }

        Color currentColor = colors[0];

        for (int i = 0; i < sentence.Length; i++)
        {
            char c = sentence[i];

            if (c == '<')
            {
                currentColor = GetColorByChar(sentence[i + 1]);
                i += 1;
                continue;
            }

            if (c == '>')
            {
                currentColor = colors[0];
                continue;
            }

            GameObject symbol = Instantiate(symbolPrefab, rectTransform);

            Image symbolImage = symbol.GetComponent<Image>();
            symbolImage.color = currentColor;

            if (c == ' ')
            {
                symbolImage.enabled = false;
            }
            else if (char.IsLetter(c))
            {
                symbolImage.sprite = symbols[char.ToUpper(c) - 'A'];
            }
            else if (char.IsNumber(c))
            {
                symbolImage.sprite = symbols[26 + c - '0'];
            }

            switch (c)
            {
                case '.': symbolImage.sprite = symbols[36]; break;
                case ',': symbolImage.sprite = symbols[37]; break;
                case '?': symbolImage.sprite = symbols[38]; break;
                case '!': symbolImage.sprite = symbols[39]; break;
                case '+': symbolImage.sprite = symbols[40]; break;
                case '-': symbolImage.sprite = symbols[41]; break;
                case '*': symbolImage.sprite = symbols[42]; break;
                case '/': symbolImage.sprite = symbols[43]; break;
                case '(': symbolImage.sprite = symbols[44]; break;
                case ')': symbolImage.sprite = symbols[45]; break;
            }

            for (int j = 0; j < frameCount; j++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void Clear()
    {
        foreach (Transform child in rectTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetWidthHeight(int widthInCharacters, int heightInCharacters)
    {
        rectTransform.sizeDelta = new Vector2(widthInCharacters * 8, heightInCharacters * 8);
    }

    public Color GetColorByChar(char c) // colors are: white, gray, rainbow
    {
        switch (c)
        {
            case 'W': return colors[0];
            case 'G': return colors[1];
            case 'r': return colors[2];
            case 'o': return colors[3];
            case 'y': return colors[4];
            case 'g': return colors[5];
            case 'b': return colors[6];
            case 'd': return colors[7];
            case 'p': return colors[8];
            default:
                break;
        }

        return colors[0];
    }
}
