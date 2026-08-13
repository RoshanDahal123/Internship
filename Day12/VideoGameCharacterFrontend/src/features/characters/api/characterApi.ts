import { httpClient } from '../../../api/httpClient';
import type { Character, CreateCharacterRequest, UpdateCharacterRequest } from '../../../types/character.types';

const ROUTE= '/api/VideoGameCharacter';
export const characterApi={
    getAll:()=>httpClient.get<Character[]>(ROUTE),
    getById:(id:number)=>httpClient.get<Character>(`${ROUTE}/${id}`),
    create:(data:CreateCharacterRequest)=>httpClient.post<Character>(ROUTE,data),
    update:(id:number,data:UpdateCharacterRequest)=>httpClient.put<void>(`${ROUTE}/${id}`, data),
    delete:(id:number)=>httpClient.delete<void>(`${ROUTE}/${id}`)
}
