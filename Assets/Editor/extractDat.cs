using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class extractDat : EditorWindow
{
    static string path = "";
    static bool Model;
    static bool Animation;
    static string type;
    static string length;
    static int startOFFSET;
    static int endOFFSET;
    static string savePATH;
    static int count = 0;
    //static enum FileType {branch_Compiled_Model,MD_ModelFile };
    [MenuItem("God Hand/.DAT specs")]

    static void Init()
    {
        path = EditorUtility.OpenFilePanel("","","");
        Model = false;
        Animation = false;


        GetWindow(typeof(extractDat));
    }
    public void OnGUI()
    {
        if (GUILayout.Button("Load .DAT"))
        {
            Init();
        }
        EditorGUILayout.LabelField("status:" + Path.GetFileNameWithoutExtension(path));





        if (path != null)
        {
            EditorGUILayout.LabelField("path: " + path);
            byte[] sample = File.ReadAllBytes(path);
            //scr
            for (int i = 0; i < sample.Length; i++)
            {
                if (sample[i] == 115 && sample[i + 1] == 99 && sample[i + 2] == 114 && sample[i + 3] == 0)
                {
                    Model = true;
                    //Debug.Log(i);
                }
                //mtb3Z
                if (sample[i] == 109 && sample[i + 1] == 116 && sample[i + 2] == 98 && sample[i + 3] == 51 && sample[i + 4] == 90)
                {
                    Animation = true;
                }
            }

            //SCR start offset is at 0x08
            //SCR end length is at 0x0C

            EditorGUILayout.LabelField("isModel: " + Model);
            EditorGUILayout.LabelField("isAnimation: " + Animation);


            List<byte> startOffset = new List<byte>();
            List<byte> finishLength = new List<byte>();
            #region type
            if (Model)
            {
                type = "MD Model";
                if (Animation)
                {
                    type = "Packed DAT file";
                }
            }
            else if (!Model && !Animation)
            {
                type = "could not be identified!";
            }
            EditorGUILayout.LabelField("type: " + type);
            #endregion
            #region findOffset
            for (int i = 0; i < 4; i++)
            {
                byte a = sample[8 + i];
                int g = System.Convert.ToByte(sample[8 + i]);
                startOffset.Add(sample[8 + i]);
            }
            length = System.BitConverter.ToString(startOffset.ToArray());
            EditorGUILayout.LabelField("start offset: " + length);
            #endregion
            #region findLength
            for (int i = 0; i < 4; i++)
            {
                byte a = sample[12 + i];
                int g = System.Convert.ToByte(sample[12 + i]);
                finishLength.Add(sample[12 + i]);
            }
            length = System.BitConverter.ToString(finishLength.ToArray());
            EditorGUILayout.LabelField("length offset: " + length);
            #endregion


            if (GUILayout.Button("Extract MD Model"))
            {
                Extract(startOffset.ToArray(), finishLength.ToArray());
            }
        }
    }
    public void Extract(byte[] start, byte[] end)
    {
        string newFile = Path.GetFileNameWithoutExtension(path);
        savePATH = EditorUtility.SaveFilePanel("", "", newFile, "md");

        byte[] file = File.ReadAllBytes(path);
        //Debug.Log(file.Length);

        int startPos = System.BitConverter.ToInt32(start, 0);
        int endPos = System.BitConverter.ToInt32(end, 0);
        //Debug.Log(startPos + "    " + endPos);

        List<byte> writeFinal = new List<byte>();
        for (int i = startPos; i < endPos; i++)
        {
            writeFinal.Add(file[i]);
        }
        File.Create(savePATH).Dispose();
        File.WriteAllBytes(savePATH, writeFinal.ToArray()); 
        //Write(writeFinal.ToArray(),0,writeFinal.Count);
        


        //for (int i = 0; i < file.Length;i++)
        //{
            //bool reachFinish = false;
        //    int start = System.BitConverter.ToInt32(startOffset.ToArray(),0);
        //    Debug.Log(start);
        //}
    }

    }
