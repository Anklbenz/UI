using UnityEngine;
public class FpsCounter : MonoBehaviour {
	[SerializeField] private int frameRange = 60;
	[SerializeField] private int fontSize = 15;
	[SerializeField] private Color color = Color.green;
	public int averageFPS { get; private set; }

	private GUIStyle _guiStyle;
	private int[] _fpsBuffer;
	private int _fpsBufferIndex;

	private void Awake() =>
			_guiStyle = new GUIStyle {fontSize = fontSize, normal = {textColor = color}};

	private void Update() {
		if (_fpsBuffer == null || frameRange != _fpsBuffer.Length)
			InitializeBuffer();

		UpdateBuffer();
		CalculateAverageFps();
	}

	private void InitializeBuffer() {
		if (frameRange <= 0)
			frameRange = 1;

		_fpsBuffer = new int[frameRange];
		_fpsBufferIndex = 0;
	}

	private void UpdateBuffer() {
		_fpsBuffer[_fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
		if (_fpsBufferIndex >= frameRange)
			_fpsBufferIndex = 0;
	}

	private void CalculateAverageFps() {
		int sum = 0;

		for (int i = 0; i < frameRange; i++) {
			int fps = _fpsBuffer[i];
			sum += fps;
		}
		averageFPS = sum / frameRange;
	}

	private void OnGUI() =>
			GUILayout.Label($" {averageFPS} fps", _guiStyle);
}