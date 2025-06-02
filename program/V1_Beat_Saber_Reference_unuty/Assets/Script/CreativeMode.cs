using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CreativeMode : MonoBehaviour
{

    public AudioSource audioForPlay;
    public bool modeStart = false;
    private List<NoteData> noteList = new List<NoteData>();

    class NoteData
    {
        public float time;       // 음악 타이밍(초)
        public int spawnPoint;   // 스폰 위치(1~4)
        public int direction;    // 방향(임시 0)
        public int colorIndex;   // 컬러(임시 0)
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (modeStart && audioForPlay.isPlaying)
        {
            // QWER 입력 감지
            if (Input.GetKeyDown(KeyCode.Q)) RecordNote(1);
            if (Input.GetKeyDown(KeyCode.W)) RecordNote(2);
            if (Input.GetKeyDown(KeyCode.E)) RecordNote(3);
            if (Input.GetKeyDown(KeyCode.R)) RecordNote(4);
        }

        if (modeStart && !audioForPlay.isPlaying && noteList.Count > 0)
        {
            SaveNotesToCSV();
            Debug.Log("패턴 CSV 저장 완료!");
            noteList.Clear(); // 다음 녹화 준비
            modeStart = false;
        }
    }

    void RecordNote(int spawnPoint)
    {
        var note = new NoteData
        {
            time = audioForPlay.time,
            spawnPoint = spawnPoint,
            direction = 0,  // 임시로 0(수정 가능)
            colorIndex = 0  // 임시로 0(수정 가능)
        };
        noteList.Add(note);
        Debug.Log($"입력: t={note.time:F2} s={note.spawnPoint}");
    }
    
    void SaveNotesToCSV()
    {
        string path = Path.Combine(Application.dataPath, "pattern.csv");
        using (StreamWriter sw = new StreamWriter(path, false))
        {
            sw.WriteLine("time,spawnPoint,direction,colorIndex");
            foreach (var note in noteList)
                sw.WriteLine($"{note.time:F2},{note.spawnPoint},{note.direction},{note.colorIndex}");
        }
    }
}
