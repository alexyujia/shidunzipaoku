using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class BeatmapManager : MonoBehaviour
{
    float OnPlayingTime = 0;
    float BeforeTime = 3;
    float BPM = 0;
    float offset = DataStorager.settings.offsetMs / 1000;
    float videoOffset = 0;
    float MaxPoint = 0;
    float NowPoint = 0;
    float MaxPlusPoint = 0;
    float NowPlusPoint = 0;
    int Combo = 0;
    int MaxCombo = 0;
    int FullCombo = 0;
    bool isPlaying = false;
    bool isVideoPlaying = false;
    bool isEnd = false;
    bool isSaved = false;
    bool isAutoPlay = DataStorager.settings.isAutoPlay;
    float distance = 10;
    public GameObject Player;
    public GameObject[] ObstacleList;
    public AudioSource MusicPlayer;
    public RawImage BackForVideo;
    public RawImage BackForImage;
    public VideoPlayer videoPlayer;
    public GameObject ComboDisplay;
    public GameObject ResultCanvas;
    public GameObject AutoPlayImage;
    public GameObject RelaxModImage;

    // 谱面信息展示
    public RawImage DisplayInfoImage;
    public TMP_Text DisplayInfoText;
    public LevelDisplayer levelDisplayer;

    // 自动游玩变量
    bool last_record = false;
    float last_change_time;
    float should_change_time;
    bool isJumped = false;
    bool ready_to_change_bpm = false;
    float should_change_bpm = 0;
    float should_change_bpm_time = 0;

    string dataFolder;

    enum B_TYPE {
        BEAT_TYPE,
        BEST_BEAT_TYPE,
        GAINT_BEAT_TYPE,
        BPM_TYPE,
        FINISH,
    }
    struct SingleBeat {
        public int type;
        public float beat_time;
        public int track;
        public int stack;
        public int rem_stack;
        public float BPM;
    }

    private List<SingleBeat> remain_beats = new();
    private List<SingleBeat> auto_remain_beats = new();

    public float getBPM(){
        return BPM;
    }

    public void LoadData(string beatmap_name){
        if(File.Exists($"{dataFolder}/{beatmap_name}/music.wav")){
            StartCoroutine(LoadMusic($"file://{dataFolder}/{beatmap_name}/music.wav", AudioType.WAV));
        } else if(File.Exists($"{dataFolder}/{beatmap_name}/music.mp3")){
            StartCoroutine(LoadMusic($"file://{dataFolder}/{beatmap_name}/music.mp3", AudioType.MPEG));
        };
        // 读取图片或视频
        if(File.Exists($"{dataFolder}/{beatmap_name}/bg.mp4")){
            videoPlayer.targetTexture = (RenderTexture)BackForVideo.texture;
            videoPlayer.playOnAwake = false;
            videoPlayer.url = $"file://{dataFolder}/{beatmap_name}/bg.mp4";
        }
        if(File.Exists($"{dataFolder}/{beatmap_name}/bg.png")){
            byte[] fileData = File.ReadAllBytes($"{dataFolder}/{beatmap_name}/bg.png");
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // 自动调整纹理大小
            BackForImage.texture = texture;
            BackForImage.GetComponent<AspectRatioFitter>().aspectRatio = (float)texture.width / texture.height;
            DisplayInfoImage.texture = texture;
            DisplayInfoImage.GetComponent<AspectRatioFitter>().aspectRatio = (float)texture.width / texture.height;
        }
        // 读取谱面
        string path = $"{dataFolder}/{beatmap_name}/data.sdz";
        if(!Directory.Exists(dataFolder)){
            Directory.CreateDirectory(dataFolder);
        }
        float last_time = 0;
        foreach( string line in File.ReadAllText(path).Split("\n")){
            string[] data = line.Split("=");
            if(data[0].Replace(" ","") == "bpm"){
                BPM = float.Parse(data[1].Replace(" ",""));
                remain_beats.Add(
                    new SingleBeat(){
                        type = (int)B_TYPE.BPM_TYPE,
                        beat_time = 0,
                        BPM = float.Parse(data[1].Replace(" ",""))
                    }
                );
                continue;
            }
            if(data[0].Replace(" ","") == "offset"){
                offset += float.Parse(data[1].Replace(" ",""));
                continue;
            }
            if(data[0].Replace(" ","") == "bg_offset"){
                videoOffset = float.Parse(data[1].Replace(" ",""));
                continue;
            }
            if(data[0].Replace(" ","") == "title"){
                DisplayInfoText.text = data[1].Replace(" ","");
                continue;
            }
            if(data[0].Replace(" ","") == "level"){
                levelDisplayer.level = float.Parse(data[1].Replace(" ",""));
                continue;
            }
            data = line.Split(",");
            if(data[0] == "D"){
                float slice_beat = float.Parse(data[3]) > 0 ? float.Parse(data[2]) / float.Parse(data[3]) : 0;
                float beat_time = last_time + (float.Parse(data[1]) + slice_beat) * (60 / BPM) + offset;
                int stack_count = int.Parse(data[5]);
                int rem_stack = 0;
                if(data.Count() >= 7){
                    rem_stack = int.Parse(data[6]);
                }
                remain_beats.Add(
                    new SingleBeat(){
                        type = (int)B_TYPE.BEAT_TYPE,
                        beat_time = beat_time,
                        track = int.Parse(data[4]),
                        stack = stack_count,
                        rem_stack = rem_stack
                    }
                );
                MaxPoint += stack_count - rem_stack;
                FullCombo += stack_count - rem_stack;
                continue;
            }
            if(data[0] == "X"){
                float slice_beat = float.Parse(data[3]) > 0 ? float.Parse(data[2]) / float.Parse(data[3]) : 0;
                float beat_time = last_time + (float.Parse(data[1]) + slice_beat) * (60 / BPM) + offset;
                int stack_count = int.Parse(data[5]);
                int rem_stack = 0;
                if(data.Count() >= 7){
                    rem_stack = int.Parse(data[6]);
                }
                remain_beats.Add(
                    new SingleBeat(){
                        type = (int)B_TYPE.BEST_BEAT_TYPE,
                        beat_time = beat_time,
                        track = int.Parse(data[4]),
                        stack = stack_count,
                        rem_stack = rem_stack
                    }
                );
                MaxPoint += stack_count - rem_stack;
                MaxPlusPoint += stack_count - rem_stack;
                FullCombo += stack_count - rem_stack;
                continue;
            }
            if(data[0] == "B"){
                float slice_beat = float.Parse(data[3]) > 0 ? float.Parse(data[2]) / float.Parse(data[3]) : 0;
                float beat_time = last_time + (float.Parse(data[1]) + slice_beat) * (60 / BPM) + offset;
                last_time = beat_time - offset;
                BPM = float.Parse(data[4]);
                remain_beats.Add(
                    new SingleBeat(){
                        type = (int)B_TYPE.BPM_TYPE,
                        beat_time = beat_time,
                        BPM = float.Parse(data[4])
                    }
                );
                continue;
            }
        }
    }

    IEnumerator LoadMusic(string path, AudioType audioType)
    {
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, audioType);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            MusicPlayer.clip = clip;
        }
    }

    // IEnumerator LoadVideo(string path)
    // {
    //     using UnityWebRequest www = UnityWebRequest.Get(path);
    //     yield return www.SendWebRequest();

    //     if (www.result == UnityWebRequest.Result.ConnectionError)
    //     {
    //         Debug.Log(www.error);
    //     }
    //     else
    //     {
    //         videoPlayer.url = path;
    //     }
    // }

    private void Awake() {
        Application.targetFrameRate = 300;

        dataFolder = $"{Application.persistentDataPath}/music";
        LoadData(BeatmapInfo.beatmap_name);
        remain_beats.Add(
            new SingleBeat(){
                type = (int)B_TYPE.FINISH,
                track = 2
            }
        );
        ComboDisplay.SetActive(false);
        ResultCanvas.SetActive(false);
        auto_remain_beats.AddRange(remain_beats);
        if(!isAutoPlay){
            AutoPlayImage.SetActive(false);
        }
        if(!DataStorager.settings.relaxMod){
            RelaxModImage.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    int[] detect_list = {
        (int)B_TYPE.BEAT_TYPE,
        (int)B_TYPE.BEST_BEAT_TYPE
    };

    // Update is called once per frame
    void FixedUpdate()
    {
        // 自动游玩
        autoplay();

        while( detect_list.Contains(remain_beats[0].type) && remain_beats[0].beat_time - OnPlayingTime + BeforeTime < 5){
            Vector3 place_pos;
            place_pos.z = (remain_beats[0].beat_time - OnPlayingTime + BeforeTime) * Player.GetComponent<Player>().GetVelocity() + Player.GetComponent<Player>().GetPos().z;
            place_pos.x = (float)((remain_beats[0].track - 2) * 3);
            for(int i = remain_beats[0].rem_stack;i < remain_beats[0].stack; i++){
                place_pos.y = i * 2;
                GameObject obs;
                switch(remain_beats[0].type){
                    case (int)B_TYPE.BEAT_TYPE: {
                        obs = Instantiate(ObstacleList[0]);
                        break;
                    };
                    case (int)B_TYPE.BEST_BEAT_TYPE: {
                        obs = Instantiate(ObstacleList[1]);
                        obs.GetComponent<MusicObstacle>().setBestNote();
                        break;
                    };
                    default: {
                        obs = Instantiate(ObstacleList[0]);
                        break;
                    }
                };
                obs.GetComponent<MusicObstacle>().setNote();
                obs.transform.position = place_pos;
                if(remain_beats[1].type == (int)B_TYPE.FINISH){
                    obs.GetComponent<MusicObstacle>().setLastNote();
                }
            }
            remain_beats.RemoveAt(0);
        }
        if(remain_beats[0].type == (int)B_TYPE.BPM_TYPE){
            remain_beats.RemoveAt(0);
        }
        if(!isVideoPlaying){
            if(-BeforeTime + OnPlayingTime >= videoOffset){
                videoPlayer.Play();
                BackForVideo.GetComponent<AspectRatioFitter>().aspectRatio = (float)videoPlayer.width / videoPlayer.height;
            }
        }

        if(BeforeTime > 0){
            BeforeTime -= Time.deltaTime;
            return;
        }
        if(!isPlaying){
            isPlaying = !isPlaying;
            MusicPlayer.Play();
        }
        if(isEnd && !isSaved){
            ResultCanvas.SetActive(true);
            if(!isAutoPlay && !DataStorager.settings.relaxMod){
                SaveResult();
            }
            isSaved = true;
        }
        OnPlayingTime = MusicPlayer.time;
    }


    public struct BeatmapResult {
        public int rating;
        public float achievement;
        public int maxCombo;
        public long achieveTime;
    }

    enum Rating {SSSp,SSS,SSp,SS,Sp,S,AAA,AA,A,BBB,BB,B,C,D,F};

    void autoplay() {
        if(auto_remain_beats[0].type == (int)B_TYPE.BPM_TYPE){
            ready_to_change_bpm = true;
            should_change_bpm_time = auto_remain_beats[0].beat_time;
            should_change_bpm = auto_remain_beats[0].BPM;
            auto_remain_beats.RemoveAt(0);
        }
        if(ready_to_change_bpm){
            if(OnPlayingTime - BeforeTime >= should_change_bpm_time){
                BPM = should_change_bpm;
                ready_to_change_bpm = false;
            }
        }
        if(isAutoPlay){
            if(!detect_list.Contains(auto_remain_beats[0].type) && auto_remain_beats[0].type != (int)B_TYPE.FINISH){
                auto_remain_beats.RemoveAt(0);
                return;
            }
            // 先判断是不是需要大跳
            if(auto_remain_beats[0].stack > 1 && !isJumped){
                float jump_should_remain_time = (float)Math.Sqrt(auto_remain_beats[0].stack * 2 / Player.GetComponent<Player>().GetGravity());
                if(OnPlayingTime + jump_should_remain_time > auto_remain_beats[0].beat_time){
                    int jump_times = (int)Math.Log(auto_remain_beats[0].stack,2);
                    for(int k = 0;k < jump_times; k++){
                        Player.GetComponent<Player>().moveUp();
                    }
                    isJumped = true;
                }
            }
            if(Player.GetComponent<Player>().GetNowTrack() != auto_remain_beats[0].track){
                if(!last_record){
                    last_change_time = OnPlayingTime - BeforeTime;
                    last_record = true;
                    float switch_time = (auto_remain_beats[0].beat_time - last_change_time) * 1 / 2;
                    if(switch_time < 0.25){
                        switch_time = 0;
                    }
                    should_change_time = last_change_time + switch_time;
                }
                if(OnPlayingTime - BeforeTime >= should_change_time){
                    int should_move_times = auto_remain_beats[0].track - Player.GetComponent<Player>().GetNowTrack();
                    // 移动
                    if(should_move_times > 0){
                        for(int j = 0; j < should_move_times; j++){
                            Player.GetComponent<Player>().moveRight();
                        }
                    } else {
                        for(int j = 0; j < -should_move_times; j++){
                            Player.GetComponent<Player>().moveLeft();
                        }
                    }
                }
            }
        }
        if(OnPlayingTime - BeforeTime >= auto_remain_beats[0].beat_time && auto_remain_beats[0].type != (int)B_TYPE.FINISH){
            if(auto_remain_beats[0].stack > 1 && isAutoPlay){
                Player.GetComponent<Player>().moveDown();
                isJumped = false;
            }

            // 设置跨越速度
            if(auto_remain_beats[1].type != (int)B_TYPE.FINISH){
                Player.GetComponent<Player>().setCrossTime(auto_remain_beats[1].beat_time - auto_remain_beats[0].beat_time);
            }

            auto_remain_beats.RemoveAt(0);
            last_record = false;
        }
    }

    void SaveResult(){
        string path = $"{Application.persistentDataPath}/record/{BeatmapInfo.beatmap_name}.dat";

        if(!Directory.Exists($"{Application.persistentDataPath}/record/")){
            Directory.CreateDirectory($"{Application.persistentDataPath}/record/");
        }

        List<BeatmapResult> data_list = new();
        if(File.Exists(path)){
            data_list = JsonConvert.DeserializeObject<List<BeatmapResult>>(File.ReadAllText(path));
        }

        BeatmapResult data = new BeatmapResult(){
            rating = GetRating(),
            achievement = GetProgress() * 100,
            maxCombo = MaxCombo,
            achieveTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond
        };

        data_list.Add(data);

        var jsonData = JsonConvert.SerializeObject(data_list.ToArray(), Formatting.Indented);
        File.WriteAllText(path,jsonData);
    }

    public float GetProgress() {
        float plus_point = MaxPlusPoint > 0 ? NowPlusPoint / MaxPlusPoint : 1;
        return (float)(NowPoint / MaxPoint +  plus_point * 0.01);
    }

    public void triggerEnd(){
        isEnd = true;
    }
    public void AddNowPoint(float point) {
        NowPoint += point;
        Combo += 1;
        MaxCombo = Math.Max(Combo,MaxCombo);
        ComboDisplay.SetActive(true);
        ComboDisplay.GetComponent<Animator>().SetTrigger("NewCombo");
    }

    public void AddNowBest(float point) {
        NowPlusPoint += point;
    }

    public void Miss() {
        Combo = 0;
        ComboDisplay.SetActive(false);
    }

    public int GetCombo() {
        return Combo;
    }

    public int GetFullCombo() {
        return FullCombo;
    }

    public int GetRating() {
        float proress = GetProgress();
        if(proress == 0){
            return (int)Rating.F;
        }
        else if(proress < 0.5){
            return (int)Rating.D;
        }
        else if(proress < 0.6){
            return (int)Rating.C;
        }
        else if(proress < 0.7){
            return (int)Rating.B;
        }
        else if(proress < 0.75){
            return (int)Rating.BB;
        }
        else if(proress < 0.8){
            return (int)Rating.BBB;
        }
        else if(proress < 0.9){
            return (int)Rating.A;
        }
        else if(proress < 0.94){
            return (int)Rating.AA;
        }
        else if(proress < 0.97){
            return (int)Rating.AAA;
        }
        else if(proress < 0.98){
            return (int)Rating.S;
        }
        else if(proress < 0.99){
            return (int)Rating.Sp;
        }
        else if(proress < 0.995){
            return (int)Rating.SS;
        }
        else if(proress < 1){
            return (int)Rating.SSp;
        }
        else if(proress < 1.005){
            return (int)Rating.SSS;
        } else {
            return (int)Rating.SSSp;
        }
    }
}
