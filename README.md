A REST API inspired by spaced repetition systems such as https://www.wanikani.com/

Currently a work in progress.

# Planned features:
- Authorisation/User Handling via Identity
- Questions, Answers, and Categorisations all creatable and configurable via API endpoints
- Full, queryable history of all user-submitted answers and their progress through the system
- Structured/Ordered reviews with time and level restrictions
- Custom reviews with no time or level restrictions
- Fuzzy matching on user input answers

# Overview

Following online spaced repetition systems like https://www.wanikani.com/, this REST API will provide a system for tracking a user's progress through a long-form progression system for learning. The system will be set up with numerous Levels to progress through over time, each with their own sets of Questions and Answers.

Generally, a user will begin interacting with this REST API through some other front end system. They will begin the first "Level", which is a group of Questions and Answers that are considered the easiest (or most essential to learn early) in the system. The user can go through this level at their own pace, though they should generally be shown the Questions in smaller groups.

Once the user has been shown a group of Questions and Answers, they should then be presented with the just the Questions, and have to input their own Answers. The strictness for this can be determined by the platform on which the user is answering the questions, e.g. Answers can be submitted one at a time as soon as the user enters a response, or can be grouped up and submitted all at once. Each Answer will also have a configurable level of Fuzzy matching, so simple errors such as typos do not hinder progress and cause frustration.

After submitting their answers for a group of Questions, these Questions will enter the spaced repetition system for the user. The user will then have to wait a period of time before they can enter an Answer to any of these Questions again. Each time they enter a correct answer, the period of time increases. Each time they enter an incorrect answer, the period of time decreases.

Eventually a user will have so long a time between their reviews for a Question that it can be considered memorised and removed from the spaced repetition system.

Once enough Questions in a Level have been memorised, the user can move on to the next level.

Levels, Questions, and Answers will all be creatable and queryable via the REST API. The time between reviews will be set on a system-wide level.

It is also not necessary that the user only interacts with the spaced repetition system. Any Question can be answered at any time, though the user's answer will not be used in the spaced repetition system.



WIP. A full write up with endpoints and examples will be provided when the project is complete enough that one can be made.


