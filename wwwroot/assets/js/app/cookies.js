const Cookies = () => {
    const cookies = document.cookie;
    const tokenMatch = cookies.match(/(^|;) ?token=([^;]*)(;|$)/);
    const token = tokenMatch ? tokenMatch[2] : null;
    return token
}