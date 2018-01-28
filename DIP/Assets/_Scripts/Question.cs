using System;
using System.Collections.Generic;


public class Question
{
    private string questionText;
    private List<string> questionOptions;
    private int answerIndex;

    public Question() : this("Default question text.", new List<string>()) {}

    public Question(string questionText) : this(questionText, new List<string>()) {}

    public Question(string questionText, List<string> questionOptions)
    {
        this.questionText = questionText;
        this.questionOptions = questionOptions;
        answerIndex = -1;
    }
    
    
}
