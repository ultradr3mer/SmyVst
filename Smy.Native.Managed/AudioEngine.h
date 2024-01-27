#pragma once
#include "EngineParameter.h"
#include <gcroot.h>
#include <map>
#include <vector>
#include "KeyData.h"

using namespace System::Collections::Generic;

public ref class AudioEngine {
public:
  AudioEngine(EngineParameter^ params);
  ~AudioEngine(); 

  void Run(Dictionary<short, int>^ currentKeys, int length, array<float*>^ outBuffer);
  static int MaxFilter = 4;
  static int MaxEnvelopes = 4;
  static int MaxEnvelopeLinks = 12;
  inline bool GetHasActiveKeys() {
    return this->activeKeys->Count > 0;
  };
private:
  Dictionary<int, float>^ InitializeNoteFrequencies();
  Dictionary<int, float>^ noteFrequencies;
  EngineParameter^ params;
  static double Wave(double saw, double t, double pow);
  void UpdateKeys(Dictionary<short, int>^ currentKeys);
  double GenerateSample(Dictionary<short, int>^ currentKeys);
  double GenerateKeys(Dictionary<short, int>^ currentKeys);
  double GenerateKey(KeyData^ data);
  double GenerateVoice(KeyData^ data, int voiceNr);
  Dictionary<int, KeyData^>^ activeKeys;
  List<short>^ keysToRemove;
};
