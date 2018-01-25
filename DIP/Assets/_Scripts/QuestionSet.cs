using System;
using System.Collections.Generic;


public class QuestionSet
{
    private DateTime experimentStarTime;
    private DateTime experimentEndTime;

    private List<Question> questions;

    public QuestionSet() : this(new List<Question>()) {}

    public QuestionSet(List<Question> questions)
    {
        this.questions = questions;
    }

}
