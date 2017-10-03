/************************************************************
■本scriptの想定use case
Kinectなどでmotion captureされたObjectの、Bone Objectを変数 = transformsに指定し、
このObject間をLineで接続する.

	■作品イメージ
		https://www.facebook.com/universaldancebr/videos/1971772456400073/
		
■LineRendererの基本的な使い方.
	・	hierarchy上にEmpty Object.
	・	これにLineRenderer componentをadd.
		Component/Effect/LineRenderer
	・	parameter set.
			useWorldSpace
				trueにすると、上記、Empty Objectの位置に関わらず、positionsで指定した座標にLineが引かれる.
				3D modelにLineを重ねる場合は、true.
				
				falseにすると、local座標になる.
				3D modelの隣(ズレた位置)にLineを表示したい場合は、false.
				
	・	本scriptをadd.

■tips
	■ねじれ問題
		LineRendererのLineは捻れが目立つ.
		unity 5.5以降で改善されているようだが、
			http://ndabecha.blogspot.jp/2016/12/unity-linerenderer-55.html
		今回、syphonを使用予定なので、unity 5.3を使用予定.
		
		対策アイデアがこちらに
			http://ndabecha.blogspot.jp/2016/06/unity-linerenderer-tips.html
			
		solution = 「逆回りで、2度描きする」です。
		
		

■基本情報
	■LineRenderer.SetPosition
		https://docs.unity3d.com/jp/540/ScriptReference/LineRenderer.SetPosition.html
		
		contents
			動的なadd comonentから、各種parameterへのaccessまで.
	
	■ラインレンダラー
		https://docs.unity3d.com/jp/530/Manual/class-LineRenderer.html
		
		contents
			基本情報
			
	■LineRendererを使う		
		http://unitygeek.hatenablog.com/entry/2013/03/25/174156
		
		contents
			基本的な使い方.
	
■その他参考
	■[アラフォーUnity] アラフォーからのUnity入門『アラフォーはラインアートがお好き。リサージュ曲線を3D空間に描く』
		http://ch.nicovideo.jp/akiba-cyberspacecowboys/blomaga/ar598693
************************************************************/
using UnityEngine;
using System.Collections;


/************************************************************
下記sampleは、key downで綺麗versionと捻れversionの比較ができるようにしてある.
SetVertexCountの繰り返し呼び出しなどは、実際の使用時によくない感じなので、
Awake()の設定でx 2のvertexCountを設定し、ストレートに2度描きすること.

また、普通のLineであれば、本scriptのように2度描きが良いが、
例えば、Syphonなど使って、Dynamic materialを使用するなどした時は、
Alphaのことも考えて、1度描き、2度描きを比較・検討すること.
************************************************************/
public class LineTrace : MonoBehaviour {
	public Transform[] transforms;
	private LineRenderer lineRenderer;
	private bool b_Beautiful = false;
	
	void Awake () {
		lineRenderer = GetComponent<LineRenderer>();
		
		// lineRenderer.useWorldSpace = false; 
		// lineRenderer.SetWidth(0.2F, 0.2F);
		
		/********************
		********************/
		// lineRenderer.SetVertexCount(transforms.Length * 2); // ネジれ対策.
		lineRenderer.SetVertexCount(transforms.Length);
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.B)){ // beautiful.
			lineRenderer.SetVertexCount(transforms.Length * 2);
			b_Beautiful = true;
		}else if(Input.GetKeyDown(KeyCode.D)){ // Dirty.
			lineRenderer.SetVertexCount(transforms.Length);
			b_Beautiful = false;
		}
		
		/********************
		********************/
		for(int i=0; i<transforms.Length;i++){
			lineRenderer.SetPosition(i, transforms[i].position);
		}
		if(b_Beautiful){
			// Reverse drawing.
			for(int i=0; i<transforms.Length;i++){
				lineRenderer.SetPosition(i + transforms.Length, transforms[transforms.Length - 1 - i].position);
			}
		}
	}
}
