using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PictureMatchingController : MonoBehaviour
{
    [SerializeField]
    Image picturePart;
    Image[] pictureParts;
    
    [SerializeField]
    GameObject picturePanel;
    GameObject[] picturePanels;

    [SerializeField]
    GameObject nullPanel;
    GameObject nullPanelIns;

    [SerializeField]
    ParticleSystem lightParticle;
    ParticleSystem[] lightParticles;

    [SerializeField]
    GameObject particles;

    Transform canvasTransform;

    int PanelCount;
    int ImageNum;

    void Start()
    {
        PanelCount = 0;
        this.name = "PictureMatching";
        Initialized();
    }

    void Update()
    {
        if(PanelCount == 9)
        {
            Invoke(nameof(Clear) , 0.5f);
            Invoke(nameof(Finish) , 3.5f);
        }
        if(Input.GetKeyDown(KeyCode.B))
		{
            Debug.Log($"{PanelCount}");
        }
        if(Input.GetKeyDown(KeyCode.C)) PanelCount++;
    }
    void Initialized()
	{
        canvasTransform = GameObject.Find("Canvas").transform;

        nullPanelIns = Instantiate(nullPanel , canvasTransform);
        nullPanelIns.name = "NullPanel";

        pictureParts = new Image[9];
        picturePanels = new GameObject[9];
        lightParticles = new ParticleSystem[9];

        for(int i = 0 ;i < 9 ;i++)
        {
            int x = i % 3 - 2;
            int y = i / 3 - 1;

            picturePanels[i] = Instantiate(picturePanel , canvasTransform);
            picturePanels[i].transform.localPosition = new Vector2(120 + 120 * x , -( 120 * y ));
            picturePanels[i].name = $"picturePanel{i + 1}";
        }

        var particleObject = Instantiate(particles).transform;
        ImageNum = UnityEngine.Random.Range(0 , 10) % 2;

        for(int i = 0; i < 9; i++)
		{
            int x = i % 3 - 2;
            int y = i / 3 - 1;

            int dif = UnityEngine.Random.Range(-30 , 30);

            pictureParts[i] = Instantiate(picturePart , canvasTransform);
            pictureParts[i].GetComponent<PictureController>().targetPanel = nullPanelIns;
            pictureParts[i].GetComponent<PictureController>().targetName = picturePanels[i].name;
            pictureParts[i].GetComponent<PictureController>().targetNum = i;
            pictureParts[i].rectTransform.anchoredPosition = new Vector2(120 + 120 * x + dif, -(120 * y) + dif);
            pictureParts[i].name = $"picture{i + 1}";
            pictureParts[i].GetComponent<Image>().sprite = GetSprite(SetImages(ImageNum) + ( i + 1 ).ToString() + ".jpg");
            pictureParts[i].GetComponent<PictureController>().isStart = true;
            pictureParts[i].GetComponent<PictureController>().targetController = nullPanelIns.GetComponent<PicturePanelController>();

            lightParticles[i] = Instantiate(lightParticle , particleObject);
            lightParticles[i].transform.position = new Vector3(65 + 65 * x , -(65 * y) , 200);
        }
	}
    Sprite GetSprite(string imagePath)
	{
        string url = Application.dataPath + "/Images/" + imagePath;
        var bytes = File.ReadAllBytes(url);
        Texture2D texture = new Texture2D(4 , 4 , TextureFormat.RGBA32 , false);
        texture.LoadImage(bytes);
        Sprite sp = Sprite.Create(texture , new Rect(0 , 0 , texture.width , texture.height) , new Vector2(0.5f , 0.5f));
        return sp;
	}
    void Clear()
    {
        Debug.Log("OK");
        foreach(var obj in pictureParts) Destroy(obj);
        var img = Instantiate(picturePart , canvasTransform);
        img.rectTransform.sizeDelta = new Vector2(300 , 300);
        img.rectTransform.anchoredPosition = new Vector2(0 , 0);
        img.sprite = GetSprite(SetImages(ImageNum) + ".jpg");
    }
    void Finish()
	{
        foreach(var obj in picturePanels) Destroy(obj);
        Destroy(nullPanelIns);
        Destroy(this.gameObject);
	}
    public void PanelCountChange(bool isSubtraction)
	{
        if(isSubtraction) PanelCount--;
        else PanelCount++;
	}
    public void ParticlePlay(int particleNum,bool isSubtraction)
	{
        if(isSubtraction)
        {
            picturePanels[particleNum].GetComponent<Image>().color = new Color(255f , 255f , 255f , 100f / 255f);
        }
        else
        {
            lightParticles[particleNum].Play();
            picturePanels[particleNum].GetComponent<Image>().color = new Color(255f / 255f , 128f / 255f , 128f / 255f);
        }
	}
    public string SetImages(int num)
	{
        switch(num)
		{
			case 0:
                return "lavender";
			case 1:
                return "mountain";
            default:
                return "mountain";
        }
	}
}
