using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

//ALWAYS AS 8 GROUPS OF BITS

public class extractMD : EditorWindow
{


    static string path = "";


    [MenuItem("God Hand/.MD specs")]

    static void Init()
    {
        path = EditorUtility.OpenFilePanel("", "", "md");

        GetWindow(typeof(extractMD));
    }
    public void OnGUI()
    {
        //EditorGUILayout.TextArea("status: " + Path.GetFileNameWithoutExtension(path));4
        if (GUILayout.Button("Load .MD"))
        {
            Init();
        }
        EditorGUILayout.LabelField("status: " + Path.GetFileNameWithoutExtension(path));

        if (path != null)
        {
            EditorGUILayout.LabelField("path: " + path);
            byte[] file = File.ReadAllBytes(path);
            int boneCount = getBoneCount(file);
            EditorGUILayout.TextArea("boneCount: " + boneCount);


            for (int i = 0; i < boneCount; i += 1)
            {
                EditorGUILayout.TextArea("boneAdress: " + getBoneAdress(file, i * 4));
                //EditorGUILayout.TextArea("boneAdress: " + getBoneAdress(file, i));
            }
        }

        //for (int i = 0; i < getBoneCount(file);i++)
        //{
        //    EditorGUILayout.TextArea("boneName: " + getBoneName(file,i));
        //}



        //string boneList = 
    }

    string getBoneName(byte[] file, int index)
    {
        List<string> name = new List<string>(); //to use index for research, 1 = z_11 & etc...
        return null;
    }
    #region boneCount
    int getBoneCount(byte[] file)
    {
        List<byte> boneOffset = new List<byte>();
        for (int i = 0; i < 4; i++){
            boneOffset.Add(file[8 + i]);
        }
        int startPos = System.BitConverter.ToInt32(boneOffset.ToArray(), 0);
       return startPos;
    }
    #endregion
    #region getBoneAdress
    //0x10
    //>>bonecount
    string getBoneAdress(byte[] file,int Additive)
    {
        byte[] boneOffset = goToAdress(16 + Additive,4,file);
        string boneCount = "";
        for (var i = 0; i < boneOffset.Length; i++)
        {
            boneCount = boneCount + " " + boneOffset[i];
            Debug.Log(i);
        }
        return boneCount;
    }
    #endregion

    byte[] goToAdress(int start, int ByteAfter, byte[] file)
    {
        List<byte> boneOffset = new List<byte>();
        for (int i = 0; i < ByteAfter; i++)
        {
            boneOffset.Add(file[start + i]);
            //Debug.Log(boneOffset[i]);
        }
        return boneOffset.ToArray();
    }


    void createBones()
    {

    }

}