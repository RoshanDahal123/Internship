import { FileText } from "lucide-react";
import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router";
import {
  deleteAllEntries,
  deleteEntry,
  getFormEntries,
} from "../../api/formApi";
import {
  fetchFailure,
  fetchStart,
  fetchSuccess,
  removeAllEntries,
  removeEntry,
} from "../../store/action";
import { useAppDispatch, useAppSelector } from "../../store/hooks";

const Display = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const [deletingId, setDeletingId] = useState<number | null>(null);
  const [deletingAll, setDeletingAll] = useState<boolean>(false);
  const [deleteError, setDeleteError] = useState("");
  const { entries, loading, error } = useAppSelector((state) => state.form);

  useEffect(() => {
    const fetchEntries = async () => {
      dispatch(fetchStart());
      try {
        const data = await getFormEntries();

        //GET//api/entries
        dispatch(fetchSuccess(data));
      } catch (error) {
        dispatch(fetchFailure("Failed to load submissions"));
      }
    };
    fetchEntries();
  }, [dispatch]);

  if (loading)
    return <p className="text-center text-gray-500 mt-10">Loading...</p>;
  if (error) return <p className="text-center text-red-500 mt-10">{error}</p>;
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

  const handleClearEntry = async (id: number) => {
    setDeleteError("");
    setDeletingId(id);

    try {
      await deleteEntry(id);
      dispatch(removeEntry(id));
      console.log("Deleted item:", deletingId);
    } catch (error) {
      setDeleteError("Error while deleting.Please try again ");
      console.log(deleteError);
    } finally {
      setDeletingId(null);
    }
  };

  const handleClearAll = async () => {
    const confirmed = window.confirm(
      `Delete all ${entries.length} entries? This cannot be undone.`,
    );
    if (!confirmed) return;
    setDeleteError("");
    setDeletingAll(true);
    try {
      const deleted = await deleteAllEntries();
      dispatch(removeAllEntries());
      console.log(deleted);
    } catch (error) {
      setDeleteError("Error occured while deleting.Please try again later");
    } finally {
      setDeletingAll(false);
    }
  };

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
            >
              ✕
            </button>
            <dl className="space-y-2 text-gray-800">
              <div>
                <dt className="font-medium text-gray-500 text-sm">Name</dt>
                <dd>{entry.name}</dd>
              </div>
              <div>
                <dt className="font-medium text-gray-500 text-sm">Email</dt>
                <dd>{entry.email}</dd>
              </div>
              <div>
                <dt className="font-medium text-gray-500 text-sm">Age</dt>
                <dd>{entry.age}</dd>
              </div>
              <div>
                <dt className="font-medium text-gray-500 text-sm">Address</dt>
                <dd>{entry.address}</dd>
              </div>
               <div>
                <dt className="font-medium text-gray-500 text-sm">Date of Birth</dt>
                <dd>{new Date(entry.dateOfBirth).toLocaleDateString()}</dd>
              </div>
              <div>
                <dt className="font-medium text-gray-500 text-sm">
                  Description
                </dt>
                <dd>{entry.description || "—"}</dd>
              </div>
              <div>
                <dt className="font-medium text-gray-500 text-sm">Id</dt>
                <dd>{entry.id || "—"}</dd>
              </div>
            </dl>
             
             {entry.cvFileUrl && (
              <div className="mt-4 pt-4 border-t border-gray-100">
                
                <Link  to={entry.cvFileUrl}
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
          {deletingAll ? "Deleting..." : "Delete All"}
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
