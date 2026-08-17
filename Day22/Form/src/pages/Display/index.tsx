import { FileText } from "lucide-react";
import { useState } from "react";
import { Link, useNavigate } from "react-router";
import {
  useDeleteAllEntriesMutation,
  useDeleteFormEntryMutation,
  useGetFormEntriesQuery,
} from "../../features/formApiSlice";

const Display = () => {
  const navigate = useNavigate();
const[deleteError,setDeleteError]=useState("");
//Replaces: useEffect + dispatch(fetchStart/fetchSuccess/fetchFailure) +
  // useAppSelector(state => state.form). RTK Query fetches on mount,
  // tracks loading/error itself, and re-fetches automatically whenever
  // a mutation below invalidates the "FormEntry" LIST tag.

const{
  data:entries=[],
  isLoading,
  isError
}= useGetFormEntriesQuery();


const[deleteFormEntry,{isLoading:isDeletingOne}]= useDeleteFormEntryMutation();

const[deleteAllEntries,{isLoading:isDeletingAll}]= useDeleteAllEntriesMutation();

const handleClearEntry= async(id:number)=>{
  setDeleteError("");
  try{
    await deleteFormEntry(id).unwrap();
  }
  catch(error){
    setDeleteError("Error while deleting. Please Try again")
  }
}

const handleClearAll= async()=>{
  const confirmed = window.confirm(
      `Delete all ${entries.length} entries? This cannot be undone.`
    );
    if (!confirmed) return;

    setDeleteError("");

    try{
      await deleteAllEntries().unwrap();
    }catch(err){
      setDeleteError("Error occured whike deketing. Please try again later");
    }
}



  if(isLoading)
    return <p className="text-center text-gray-500 mt-10">Loading...</p>;

  if (isError) return <p className="text-center text-red-500 mt-10">Failed to load submissions.</p>;


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


  type DetailItemProps = {
  label: string;
  value: React.ReactNode;
};

  const DetailItem=({label,value}:DetailItemProps)=>{
    return <div>
      <dt className="font-medium text-gray-500 text-sm">
        {label}
      </dt>
      <dd>{value}</dd>
    </div>
  }

  

  return (
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
        {entries.map((entry) => (
          <div
            key={entry.id}
            className="bg-white rounded-xl shadow-md border border-gray-100 p-5 relative"
          >
            <button
              className="absolute top-3 right-3 text-gray-400 hover:text-red-500 text-sm"
              aria-label="Delete entry"
              onClick={() => {
                handleClearEntry(entry.id);
              }}
              disabled={isDeletingOne}
            >
              ✕
            </button>
            <dl className="space-y-2 text-gray-800">
              <DetailItem label="Name" value={entry.name} />
              <DetailItem label="Email" value={entry.email} />
              <DetailItem label="Age" value={entry.age} />
              <DetailItem label="Address" value={entry.address} />
              <DetailItem
                label="Date of Birth"
                value={new Date(entry.dateOfBirth).toLocaleDateString()}
              />
             <DetailItem label="Description"
             value={entry.description || "—"}/>
             <DetailItem label="Id"
              value={entry.id || "—"}/>
            </dl>

            {entry.cvFileUrl && (
              <div className="mt-4 pt-4 border-t border-gray-100">
                <Link
                  to={entry.cvFileUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="inline-flex items-center gap-1.5 text-sm text-blue-600 hover:text-blue-700 font-medium"
                >
                  <FileText className="w-4 h-4" /> View CV
                </Link>
              </div>
            )}

            {entry.education.length > 0 && (
              <div className="mt-4 pt-4 border-t border-gray-100">
                <h4 className="font-medium text-gray-500 text-sm mb-2">
                  Education
                </h4>
                <ul className="space-y-2">
                  {entry.education.map((edu, i) => (
                    <li
                      key={i}
                      className="text-sm text-gray-800 bg-gray-50 rounded-lg p-2"
                    >
                      <span className="font-medium">{edu.degree}</span> —{" "}
                      {edu.institutionName} ({edu.year})
                    </li>
                  ))}
                </ul>
              </div>
            )}
          </div>
        ))}
      </div>

      {entries.length > 1 && (
        <button
          className="w-full mt-6 py-2.5 px-4 bg-gray-600 hover:bg-gray-700 text-white font-medium rounded-lg"
          onClick={handleClearAll}
        >
          {isDeletingAll ? "Deleting..." : "Delete All"}
        </button>
      )}

      {deleteError && (
        <p role="alert" className="text-red-500 text-sm mb-4">
          {deleteError}
        </p>
      )}
    </div>
  );
};

export default Display;
