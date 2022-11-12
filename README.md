This is a simulation of genetics, population dynamics/ecology and evolution.
It is made in Unity and uses artificial neural networks created with ml-agents to control the
organisms.

To properly train the agents you need to write this(with your own run names and config files,or choose one that works, right now i'd recommend memtest.yaml) 
in the terminal:


mlagents-learn config/YOUR_CONFIG_FILE.yaml --time-scale=1 --run-id=YOUR_RUN_ID 

Your python environment should be in the Py folder.

The relevant prefabs to mess around with if you want to change the values of the organisms are blib, brainblob, brainblyb and brainblub. Which are all located in blob_bucket_updated/Assets/Prefabs. 

If you know even less about what you're doing than I do, which is quite a feat, you should probably not mess with the BrainBlob, BrainBlyb or BrainBlub scripts since then you'd need to change a lot of other stuff related to the structure of the neural networks...

A lot of the values in the "X"-controls and "X"-genome scripts are generated from their genomes at runtime, so you might not be able to change all of them in the editor. 
