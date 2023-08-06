export class Map {
    id: string = ''
    title: string = ''
    description: string = ''
    path: string = ''
    isVisible: boolean = false
    levels : Array<Level> = []
    professorId : string = ''

    constructor(id: string, title: string, description: string, path: string, isVisible: boolean, levels: Array<Level>) { 
        this.levels = levels; 
        this.title = title;
        this.description = description;
        this.path = path;
        this.isVisible = isVisible;
        this.id = id;
    }
}

export class PlayerTask {
    id: string =''
    assignmentId : string = ''
    playerId : string = ''
    scorePoints: number = 0
    percentage: number = 0
    answers : string = ''
    assignment!: Assignment 
}

export class Level {
    id: string = ''
    title: string = ''
    description: string = ''
    assignments : Array<Assignment> = []
    number : number = 0

    constructor(tasks: Array<Assignment>) { this.assignments = tasks; }
}

export class Assignment {
    id: string = ''
    title: string = ''
    description: string = ''
    isCoding: boolean = true;
    hasBadge: boolean = false;
    hasArgs: boolean = false;
    isTimeMeasured: boolean = false;
    testCases : Array<TestCase> = []
    answers : Array<Answer> = []
    points: number = 0
    initialCode : string = ''
    seconds : number = 0
    isMultiSelect: boolean = false
    badgeId: string = ''
    number : number = 0
    levelId :  string = ''
}

export class TestCase {
    id: string = ''
    input: string = ''
    output: string = ''

  }

export class Answer {
    id: string = ''
    offeredAnswer: string = ''
    isCorrect: boolean = false

}

export class Badge {
    path: string = ''
    id: string = ''
}