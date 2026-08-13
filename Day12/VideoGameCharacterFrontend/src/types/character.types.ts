
//Mirrors CharacterResponse from the Api

export interface Character{
    id: number;
    name:string;
    game:string;
    role:string
}
// Mirrors CreateCharacterRequest
export interface CreateCharacterRequest {
  name: string;
  game: string;
  role: string;
}

// Mirrors UpdateCharacterRequest
export interface UpdateCharacterRequest {
  id:number;
  name: string;
  game: string;
  role: string;
}


