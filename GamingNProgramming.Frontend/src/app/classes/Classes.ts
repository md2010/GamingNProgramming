export class Map {
    id: string = ''
    title: string = ''
    description: string = ''
    path: string = ''
    isVisible: boolean = false
    levels : Array<Level> = []

    constructor(title: string, description: string, path: string, isVisible: boolean, levels: Array<Level>) { 
        this.levels = levels; 
        this.title = title;
        this.description = description;
        this.path = path;
        this.isVisible = isVisible;
    }
}


export class Level {
    title: string = ''
    description: string = ''
    assignments : Array<Assignment> = []

    constructor(tasks: Array<Assignment>) { this.assignments = tasks; }
}

export class Assignment {
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
    badgeId!: string  
   
}

export class TestCase {
    input: string = ''
    output: string = ''

  }

export class Answer {
    offeredAnswer: string = ''
    isCorrect: boolean = false

}

export class Badge {
    path: string = ''
    id: string = ''
}