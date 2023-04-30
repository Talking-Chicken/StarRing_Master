using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*this is the enum of input/output type*/
public enum InformationType {Location, Time, Testimony, Statement}

/*this is information comment that will be drawn on the side of information node when player want to view details*/
public struct InformationComment {
    public string CommentName;
    public Image CommentImage;
}

/*this is information content*/
public struct InformationContent {
    public string ContentName, ContentShortDes, ContentLongDes;
    public Image ContentImage;
}

/*this is the pin that reciving outputs*/
public class InformationInput {
    public Information Info;
}

/*this is the pin that giving output to input pin*/
public class InformationOutput {
    public Information Info;
}

/*this is information used in logic map
  it stores information name, content, input, output, comment
  content and comment can change as player progress the game*/
[CreateAssetMenu(fileName = "Information", menuName = "Star Ring/Information", order = 2)]
public class Information : ScriptableObject
{
    public InformationType Type;
    public string InformationName;
    public InformationContent Content;
    public InformationComment Comment;
}