import { useNavigate } from "react-router";
import { clearForm, deleteEntry } from "../../store/action";
import { useAppDispatch, useAppSelector } from "../../store/hooks";



const Display=()=>{
  const navigate=useNavigate();
  const dispatch= useAppDispatch();
 const entries= useAppSelector(store=>store.form.entries);

 if (entries.length === 0) {
    return (
      <div className="max-w-md mx-auto p-6 text-center">
        <p className="text-gray-600 mb-4">No submissions yet.</p>
        <button
          onClick={() => navigate("/")}
          className="py-2 px-4 bg-blue-600 hover:bg-blue-700 text-white rounded-lg"
        >
          Go to form
        </button>
      </div>
    );
  }

 const handleReset=()=>{
  dispatch(clearForm());
  navigate("/")
 };


 return(
   <div className="max-w-2xl mx-auto p-6">
      <div className="flex justify-between items-center mb-6">
         <h2 className="text-2xl font-bold text-gray-800">
          Submitted Information ({entries.length})
        </h2>
         <button
          onClick={() => navigate("/")}
          className="py-2 px-4 bg-blue-600 hover:bg-blue-700 text-white rounded-lg text-sm"
        >
          + Add another
        </button>
      </div>

         <div className="space-y-4">
          {entries.map((entry)=>(
            <div
             key={entry.id}
            className="bg-white rounded-xl shadow-md border border-gray-100 p-5 relative">
               <button
              onClick={() => dispatch(deleteEntry(entry.id))}
              className="absolute top-3 right-3 text-gray-400 hover:text-red-500 text-sm"
              aria-label="Delete entry"
            >
              ✕
            </button>
              <dl className="space-y-2 text-gray-800">
              <div><dt className="font-medium text-gray-500 text-sm">Name</dt><dd>{entry.name}</dd></div>
              <div><dt className="font-medium text-gray-500 text-sm">Email</dt><dd>{entry.email}</dd></div>
              <div><dt className="font-medium text-gray-500 text-sm">Age</dt><dd>{entry.age}</dd></div>
              <div><dt className="font-medium text-gray-500 text-sm">Address</dt><dd>{entry.address}</dd></div>
              <div><dt className="font-medium text-gray-500 text-sm">Description</dt><dd>{entry.description || "—"}</dd></div>
              <div><dt className="font-medium text-gray-500 text-sm">Id</dt><dd>{entry.id|| "—"}</dd></div>
            </dl>
            </div>
          ))
          }

         </div>
         {entries.length > 1 && (
        <button
          onClick={() => dispatch(clearForm())}
          className="w-full mt-6 py-2.5 px-4 bg-gray-600 hover:bg-gray-700 text-white font-medium rounded-lg"
        >
          Clear all
        </button>
      )}
      </div>
 )

}

export default Display;