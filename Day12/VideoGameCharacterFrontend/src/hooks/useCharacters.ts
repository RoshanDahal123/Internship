import { useCallback, useEffect, useState } from "react";
import { ApiError } from "../api/httpClient";
import { characterApi } from "../features/characters/api/characterApi";
import type { Character, CreateCharacterRequest, UpdateCharacterRequest } from "../types/character.types";


interface UseCharacterResult{
    characters:Character[];
    isLoading:boolean;
    error:string|null;
    createCharacter:(data:CreateCharacterRequest)=>Promise<void>;
    updateCharacter:(id:number,data:UpdateCharacterRequest)=>Promise<void>;
    deleteCharacter: (id: number) => Promise<void>;
     refetch: () => Promise<void>;
}

export function useCharacters():UseCharacterResult{
    const[characters, setCharacters]=useState<Character[]>([]);
    const[isLoading, setIsLoading]=useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const fetchCharacters = useCallback(async ()=>{
    setIsLoading(true);
    setError(null);
    try{
        const data= await characterApi.getAll();
        setCharacters(data);
    }catch(err){
        setError(err instanceof ApiError ?err.message:"Failed to load characters");
    }finally{
        setIsLoading(false);
    }
 },[])
  useEffect(()=>{
    fetchCharacters();
  },[fetchCharacters])


  const createCharacter= useCallback(async (data:CreateCharacterRequest)=>{
    const created= await characterApi.create(data);
    setCharacters((prev)=>[...prev,created]);
  },[]);

 const updateCharacter= useCallback(async(id: number, data: UpdateCharacterRequest)=>{
await characterApi.update(id,data);
setCharacters((prev)=>prev.map( c=>c.id === id? {...c,...data}:c))
 },[])

 const deleteCharacter= useCallback(async (id:number)=>{
    await characterApi.delete(id);
    setCharacters((prev)=>prev.filter((c)=>c.id!==id));
 },[])

 return {characters,isLoading, error,createCharacter,updateCharacter, deleteCharacter, refetch:fetchCharacters}
}