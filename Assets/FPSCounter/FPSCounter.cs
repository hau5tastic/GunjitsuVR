using UnityEngine;

public class FPSCounter : MonoBehaviour {

	public int AverageFPS { get; private set; }
	public int HighestFPS { get; private set; }
	public int LowestFPS { get; private set; }

	public int frameRange = 60;

	int[] fpsBuffer;
	int fpsBufferIndex;

	void InitializeBuffer () {
		// Make sure that frameRange is at least 1, and set the index to 0.
		if (frameRange <= 0) {
			frameRange = 1;
		}
		fpsBuffer = new int[frameRange];
		fpsBufferIndex = 0;
	}

	void Update() {
		// For any runtime changes to frameRange
		if (fpsBuffer == null || fpsBuffer.Length != frameRange) {
			InitializeBuffer();
		}
		UpdateBuffer();
		CalculateFPS();
	}

	void UpdateBuffer () {
		// 1 Frame per Update() = 1 frame / x seconds

		// Don't use deltaTime since it is affected by Unity's current time scale. Use unscaledDeltaTime instead.
		fpsBuffer[fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
		if (fpsBufferIndex >= frameRange) {
			fpsBufferIndex = 0;
		}
	}

	void CalculateFPS () {
		int sum = 0;
		int highest = 0;
		int lowest = int.MaxValue;
		for (int i = 0; i < frameRange; i++) {
			int fps = fpsBuffer[i];
			sum += fps;
			if (fps > highest) {
				highest = fps;
			}
			if (fps < lowest) {
				lowest = fps;
			}
		}
		AverageFPS = sum / frameRange;
		HighestFPS = highest;
		LowestFPS = lowest;
	}
}
