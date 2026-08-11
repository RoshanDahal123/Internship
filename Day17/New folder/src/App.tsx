import { useAppDispatch, useAppSelector } from './app/hooks';

import { amountAdded } from './features/counter/counter-slice';

import { useFetchBreedsQuery } from './features/dogs/dogs-api-slice';


const App = () => {
  const value =useAppSelector(store=>store.counter.value);
  const dispatch= useAppDispatch();

  const {data=[], isFetching}=useFetchBreedsQuery('retriever');
  console.log(data);
  const handleClick=()=>{
    //increment by 1
    //dispatch(incremented());

    //increment by a fixed amount
    
     dispatch(amountAdded(3));
  }
  return (
    <div>
      <header>
       <p>
        <button onClick={handleClick}> count is:{value}</button>
       </p>

       <div>
       <p> Number of dogs fetched:{data.length}</p>

       <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Picture</th>
          </tr>
          
        </thead>
        <tbody>
          {data.map((breed)=>(
            <tr key={breed.id}>
              <td>{breed.name}</td>
              <td>
                <img src={breed.image_link} alt="{breed.name}" height={250} />
              </td>
            </tr>
          ))}
        </tbody>
       </table>
       </div>
       


      </header>
    </div>
  )
}

export default App