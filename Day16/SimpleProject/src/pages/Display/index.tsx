type DisplayProps={
    formData:{
        name:string
    }
}
function Display({formData}:DisplayProps){
    return <>
    <div> This is the form data:{formData.name}</div>
    </>
}
export default Display;