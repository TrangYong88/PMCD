Users (UserActions, UserClasses, ClassTestUsers, ClassTestUserQuestions, ClassTestUserQuestionAnswers)
Questions(Answers, QuestionCurriculumns, ClassTestQuestions, ClassTestUserQuestions, ClassTestUserQuestionAnswers)
Curriculums()
Classes()
Answers()



DELETE FROM ClassTestUserQuestionAnswers
WHERE  ClassTestUserQuestionAnswers.ClassTestUserQuestionId =
(SELECT ClassTestUserQuestionAnswers.ClassTestUserQuestionId 
FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestUsers,UserClasses, Classes, Curriculums
WHERE ClassTestUserQuestions.ClassTestUserQuestionId=ClassTestUserQuestionAnswers.ClassTestUserQuestionId
AND ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId
AND UserClasses.ClassUserId = ClassTestUsers.ClassUserId
AND Classes.ClassId = UserClasses.ClassId
AND Curriculums.CurriculumId = Classes.CurriculumId 
AND Curriculums.CurriculumId = 2)




DELETE FROM ClassTestUserQuestionAnswers
WHERE  ClassTestUserQuestionAnswers.ClassTestUserQuestionId =
(SELECT ClassTestUserQuestionAnswers.ClassTestUserQuestionId 
FROM ClassTestUserQuestionAnswers, ClassTestUserQuestions, ClassTestUsers,UserClasses, Users
WHERE ClassTestUserQuestions.ClassTestUserQuestionId = ClassTestUserQuestionAnswers.ClassTestUserQuestionId
AND ClassTestUsers.ClassTestUserId = ClassTestUserQuestions.ClassTestUserId
AND UserClasses.ClassUserId = ClassTestUsers.ClassUserId
AND Users.UserId = UserClasses.UserId
