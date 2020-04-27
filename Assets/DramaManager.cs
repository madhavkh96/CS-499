using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DramaManager : MonoBehaviour
{
    public Dictionary<string, Storylet> storylets = new Dictionary<string, Storylet>();

    public static DramaManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(this);
        }

        StorySetup();

    }




    void StorySetup() {
        List<StoryletMotivation> storyMotivations = new List<StoryletMotivation>();
        Storylet storylet1 = new Storylet("Introduction", StoryArc.ACTI, StoryletMotivation.Adventure);
        storylet1.SetText("The Tale was long");
        storylets.Add(storylet1.GetName() , storylet1);
        
        Storylet storylet2 = new Storylet("FirstPath", StoryArc.ACTI, StoryletMotivation.Greedy);
        storylet2.SetText("Made Greedy Choice");
        storylets.Add(storylet2.GetName(), storylet2);

        Storylet storylet3 = new Storylet("MoralPath", StoryArc.ACTI, StoryletMotivation.Kindness);
        storylet3.SetText("Made a choice full of Kindness at the cost of his health");
        storylets.Add(storylet3.GetName(), storylet3);

        storyMotivations.Add(StoryletMotivation.Greedy);
        storyMotivations.Add(StoryletMotivation.Adventure);
        Storylet storylet4 = new Storylet("GreedSecondPath", StoryArc.ACTI, storyMotivations);
        storyMotivations.Clear();
        storylet4.SetText("Made a greedy choice full of adventure");
        storylets.Add(storylet4.GetName(), storylet4);


        storyMotivations.Add(StoryletMotivation.Adventure);
        storyMotivations.Add(StoryletMotivation.Love);
        Storylet storylet5 = new Storylet("LoveSecondPath", StoryArc.ACTI, storyMotivations);
        storyMotivations.Clear();
        storylet5.SetText("The choice which showed his kindness at the expense of his health and immediate return with adventure");
        storylets.Add(storylet5.GetName(), storylet5);

        Storylet storylet6 = new Storylet("Loveonly", StoryArc.ACTI, StoryletMotivation.Love);
        storylet6.SetText("Love.... what we all do for it");
        storylets.Add(storylet6.GetName(), storylet6);

        Storylet storylet7 = new Storylet("Greedy", StoryArc.ACTI, StoryletMotivation.Greedy);
        storylet7.SetText("Greed, is it really all that bad?");
        storylets.Add(storylet7.GetName(), storylet7);

    }


    void StoryFill() { 
        


    }


    void AIMatching() { 
        

    }

}
